using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.Infrastructure.Abstractions;
using DailyTool.ViewModels.Navigation;
using System.Collections.ObjectModel;

namespace DailyTool.ViewModels.People
{
    public class PeopleOverviewViewModel : ObservableObject, INavigationTarget
    {
        private readonly IPersonService _personService;
        private readonly IMapper<PersonModel, PersonViewModel> _viewModelMapper;
        private readonly INavigationService _navigationService;

        private AddPersonViewModel? _addPersonViewModel;
        private PersonViewModel? _selectedPerson;
        private ObservableCollection<PersonViewModel> _people = new ObservableCollection<PersonViewModel>();

        public PeopleOverviewViewModel(
            IPersonService personService,
            IMapper<PersonModel, PersonViewModel> viewModelMapper,
            INavigationService navigationService)
        {
            _personService = personService ?? throw new ArgumentNullException(nameof(personService));
            _viewModelMapper = viewModelMapper ?? throw new ArgumentNullException(nameof(viewModelMapper));
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
            set
            {
                SetProperty(ref _people, value);
            }
        }

        public async Task LoadDataAsync()
        {
            var people = await _personService.GetAllAsync().ConfigureAwait(true);
            var mappedPeople = people.Select(_viewModelMapper.Map);
            People = new ObservableCollection<PersonViewModel>(mappedPeople);
        }

        public Task OnNavigatedToAsync(IReadOnlyDictionary<string, string> parameters, NavigationMode navigationMode)
        {
            return Task.CompletedTask;
        }

        public Task<bool> OnNavigatingFromAsync(NavigationMode navigationMode)
        {
            return Task.FromResult(true);
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
            AddPersonViewModel = null;
            return Task.CompletedTask;
        }

        private bool CanRemovePerson()
        {
            return SelectedPerson is not null;
        }

        private Task RemovePersonAsync()
        {
            if (SelectedPerson is null)
            {
                return Task.CompletedTask;
            }

            People.Remove(SelectedPerson);
            return Task.CompletedTask;
        }
    }
}
