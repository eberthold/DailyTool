using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyTool.ViewModels.Navigation;

namespace DailyTool.ViewModels.Daily
{
    public class PeopleEditViewModel : ObservableObject, INavigationTarget
    {
        private readonly INavigationService _navigationService;

        private AddPersonViewModel? _addPersonViewModel;
        private PersonViewModel? _selectedPerson;

        public PeopleEditViewModel(
            IPeopleState state,
            INavigationService navigationService)
        {
            State = state ?? throw new ArgumentNullException(nameof(state));
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

        public IPeopleState State { get; }

        public Task LoadDataAsync()
        {
            return State.LoadDataAsync();
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

            State.People.Remove(SelectedPerson);
            return Task.CompletedTask;
        }
    }
}
