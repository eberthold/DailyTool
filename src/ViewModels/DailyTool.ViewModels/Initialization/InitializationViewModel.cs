using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyTool.BusinessLogic.Initialization;
using DailyTool.ViewModels.Abstractions;
using DailyTool.ViewModels.Navigation;

namespace DailyTool.ViewModels.Initialization
{
    public class InitializationViewModel : ObservableObject, INavigationTarget, ILoadDataAsync
    {
        private readonly IInitializationStateController _initializationStateController;
        private InitializationStageState _currentState = new();

        private AddPersonViewModel? _addPersonViewModel;
        private TimeSpan _startTime;
        private TimeSpan _endTime;

        public InitializationViewModel(IInitializationStateController initializationStateController)
        {
            _initializationStateController = initializationStateController ?? throw new ArgumentNullException(nameof(initializationStateController));

            AddPersonCommand = new AsyncRelayCommand(AddPersonAsync, CanAddPerson);
            StartMeetingCommand = new AsyncRelayCommand(StartMeetingAsync);
        }

        public IAsyncRelayCommand AddPersonCommand { get; }

        public AsyncRelayCommand StartMeetingCommand { get; }

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
            CurrentState = await _initializationStateController.GetStateAsync().ConfigureAwait(false);

            StartTime = CurrentState.MeetingInfo.MeetingStartTime;
            EndTime = StartTime.Add(CurrentState.MeetingInfo.MeetingDuration);
        }

        private bool CanAddPerson()
        {
            return IsInViewMode;
        }

        private Task AddPersonAsync()
        {
            return Task.CompletedTask;
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
            return Task.FromResult(true);
        }

        private void RefreshCommands()
        {
            AddPersonCommand.NotifyCanExecuteChanged();
            StartMeetingCommand.NotifyCanExecuteChanged();
        }
    }
}