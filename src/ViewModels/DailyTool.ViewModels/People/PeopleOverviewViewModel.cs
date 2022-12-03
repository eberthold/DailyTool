using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.Infrastructure.Abstractions;
using DailyTool.ViewModels.Navigation;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace DailyTool.ViewModels.People
{
    public class PeopleOverviewViewModel : ObservableObject, INavigationTarget
    {
        private readonly IPersonService _personService;
        private readonly IMapper _mapper;
        private readonly INavigationService _navigationService;

        private AddPersonViewModel? _addPersonViewModel;
        private PersonViewModel? _selectedPerson;
        private ObservableCollection<PersonViewModel> _people = new ObservableCollection<PersonViewModel>();

        public PeopleOverviewViewModel(
            IPersonService personService,
            IMapper mapper,
            INavigationService navigationService)
        {
            _personService = personService ?? throw new ArgumentNullException(nameof(personService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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
                foreach (var person in _people)
                {
                    person.PropertyChanged -= OnPersonChanged;
                }

                _people.CollectionChanged -= PeopleCollectionChanged;
                SetProperty(ref _people, value);
                value.CollectionChanged += PeopleCollectionChanged;

                foreach (var person in value)
                {
                    person.PropertyChanged += OnPersonChanged;
                }
            }
        }

        private void PeopleCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var person in e.OldItems?.OfType<PersonViewModel>() ?? Enumerable.Empty<PersonViewModel>())
            {
                person.PropertyChanged -= OnPersonChanged;
            }

            foreach (var person in e.NewItems?.OfType<PersonViewModel>() ?? Enumerable.Empty<PersonViewModel>())
            {
                person.PropertyChanged += OnPersonChanged;
            }
        }

        private void OnPersonChanged(object? sender, PropertyChangedEventArgs e)
        {
            var person = sender as PersonViewModel;
            if (person is null)
            {
                return;
            }

            var mappedPerson = _mapper.Map<Person>(person);
            _personService.UpdatePersonAsync(mappedPerson);
        }

        public async Task LoadDataAsync()
        {
            var people = await _personService.GetAllAsync().ConfigureAwait(true);
            var mappedPeople = people.Select(_mapper.Map<PersonViewModel>);
            People = new ObservableCollection<PersonViewModel>(mappedPeople);
        }

        public Task OnNavigatedToAsync(NavigationMode navigationMode)
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
