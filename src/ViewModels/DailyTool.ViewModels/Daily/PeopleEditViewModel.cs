using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.ViewModels.Extensions;
using DailyTool.ViewModels.Initialization;
using DailyTool.ViewModels.Navigation;
using System.Collections.ObjectModel;

namespace DailyTool.ViewModels.Daily
{
    public class PeopleEditViewModel : ObservableObject, IPeopleEditViewModel
    {
        private readonly IPersonService _personService;
        private readonly INavigationService _navigationService;

        private AddPersonViewModel? _addPersonViewModel;
        private PersonViewModel? _selectedPerson;
        private ObservableCollection<PersonViewModel> _people = new();

        public PeopleEditViewModel(
            IPersonService personService,
            INavigationService navigationService)
        {
            _personService = personService ?? throw new ArgumentNullException(nameof(personService));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            AddPersonCommand = new AsyncRelayCommand(AddPersonAsync, CanAddPerson);
            RemovePersonCommand = new AsyncRelayCommand(RemovePersonAsync, CanRemovePerson);
        }

        public IAsyncRelayCommand AddPersonCommand { get; }

        public IAsyncRelayCommand RemovePersonCommand { get; }

        public PersonViewModel? SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                if (!SetProperty(ref _selectedPerson, value))
                {
                    return;
                }

                RefreshCommands();
            }
        }

        public AddPersonViewModel? AddPersonViewModel
        {
            get => _addPersonViewModel;
            internal set
            {
                if (!SetProperty(ref _addPersonViewModel, value))
                {
                    return;
                }

                OnPropertyChanged(nameof(IsInPersonAddMode));
                OnPropertyChanged(nameof(IsInViewMode));
                RefreshCommands();
            }
        }

        public bool IsInPersonAddMode => AddPersonViewModel is not null;

        public bool IsInViewMode => AddPersonViewModel is null;

        public ObservableCollection<PersonViewModel> People
        {
            get => _people;
            set => SetProperty(ref _people, value);
        }

        public async Task LoadDataAsync()
        {
            var people = await _personService.GetAllAsync();
            var mappedPeople = people.Select(x =>
            {
                var viewModel = x.ToViewModel();
                viewModel.PropertyChanged += OnPersonChanged;
                return viewModel;
            });

            People = new ObservableCollection<PersonViewModel>(mappedPeople);
        }

        public Task OnNavigatedToAsync(NavigationMode navigationMode)
        {
            throw new NotImplementedException();
        }

        public Task<bool> OnNavigatingFromAsync(NavigationMode navigationMode)
        {
            throw new NotImplementedException();
        }

        private bool CanAddPerson()
        {
            return IsInViewMode;
        }

        private async Task AddPersonAsync()
        {
            AddPersonViewModel = await _navigationService.CreateNavigationTarget<AddPersonViewModel>();
            AddPersonViewModel.AddCloseCallback(() => OnAddPersonClosed());
        }

        private void RefreshCommands()
        {
            AddPersonCommand.NotifyCanExecuteChanged();
            RemovePersonCommand.NotifyCanExecuteChanged();
        }

        private Task OnAddPersonClosed()
        {
            if (AddPersonViewModel?.AddedPerson is null)
            {
                AddPersonViewModel = null;
                return Task.CompletedTask;
            }

            People.Add(AddPersonViewModel.AddedPerson);
            AddPersonViewModel = null;
            return Task.CompletedTask;
        }

        private bool CanRemovePerson()
        {
            return SelectedPerson is not null;
        }

        private async Task RemovePersonAsync()
        {
            if (SelectedPerson is null)
            {
                return;
            }

            SelectedPerson.PropertyChanged -= OnPersonChanged;
            await _personService.DeletePersonAsync(SelectedPerson.Id);
            People.Remove(SelectedPerson);
        }

        private void OnPersonChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var person = sender as PersonViewModel;
            if (person is null)
            {
                return;
            }

            _personService.UpdatePersonAsync(person.ToBusinessObject());
        }
    }
}
