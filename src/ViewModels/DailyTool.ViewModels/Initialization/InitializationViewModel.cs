using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyTool.BusinessLogic.Initialization;
using DailyTool.BusinessLogic.People;
using DailyTool.ViewModels.Abstractions;
using DailyTool.ViewModels.Navigation;

namespace DailyTool.ViewModels.Initialization
{
    public class InitializationViewModel : ObservableObject, INavigationTarget, ILoadDataAsync
    {
        private readonly IInitializationStateController _initializationStateController;
        private readonly INavigationService _navigationService;
        private InitializationStageState _currentState = new();

        private AddPersonViewModel? _addPersonViewModel;
        private Person? _selectedPerson;
        private TimeSpan _startTime;
        private TimeSpan _endTime;

        public InitializationViewModel(
            IInitializationStateController initializationStateController,
            INavigationService navigationService)
        {
            _initializationStateController = initializationStateController ?? throw new ArgumentNullException(nameof(initializationStateController));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            AddPersonCommand = new AsyncRelayCommand(AddPersonAsync, CanAddPerson);
            RemovePersonCommand = new AsyncRelayCommand(RemovePersonAsync, CanRemovePerson);
            StartMeetingCommand = new AsyncRelayCommand(StartMeetingAsync);
        }

        public IAsyncRelayCommand AddPersonCommand { get; }

        public AsyncRelayCommand RemovePersonCommand { get; }

        public AsyncRelayCommand StartMeetingCommand { get; }

        public Person? SelectedPerson
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

        public TimeSpan EndTime
        {
            get => _endTime;
            set
            {
                if (!SetProperty(ref _endTime, value))
                {
                    return;
                }

                CurrentState.MeetingInfo.MeetingDuration = value.Subtract(StartTime);
            }
        }

        public TimeSpan StartTime
        {
            get => _startTime;
            set
            {
                if (!SetProperty(ref _startTime, value))
                {
                    return;
                }

                CurrentState.MeetingInfo.MeetingStartTime = value;
                EndTime = StartTime.Add(CurrentState.MeetingInfo.MeetingDuration);
            }
        }

        public InitializationStageState CurrentState
        {
            get => _currentState;
            set => SetProperty(ref _currentState, value);
        }

        public async Task LoadDataAsync()
        {
            CurrentState = await _initializationStateController.GetStateAsync();

            StartTime = CurrentState.MeetingInfo.MeetingStartTime;
            EndTime = StartTime.Add(CurrentState.MeetingInfo.MeetingDuration);
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

        private Task StartMeetingAsync()
        {
            return _initializationStateController.SaveStateAsync();
        }

        public Task OnNavigatedToAsync(NavigationMode navigationMode)
        {
            return Task.CompletedTask;
        }

        public Task<bool> OnNavigatingFromAsync(NavigationMode navigationMode)
        {
            return Task.FromResult(IsInViewMode);
        }

        private void RefreshCommands()
        {
            AddPersonCommand.NotifyCanExecuteChanged();
            RemovePersonCommand.NotifyCanExecuteChanged();
            StartMeetingCommand.NotifyCanExecuteChanged();
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

            return _initializationStateController.RemovePersonAsync(SelectedPerson);
        }
    }
}