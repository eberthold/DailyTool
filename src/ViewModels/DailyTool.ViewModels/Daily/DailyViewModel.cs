using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.BusinessLogic.System;
using DailyTool.ViewModels.Abstractions;
using DailyTool.ViewModels.Navigation;

namespace DailyTool.ViewModels.Daily
{
    public class DailyViewModel : ObservableObject, INavigationTarget, ILoadDataAsync, IDisposable
    {
        private readonly IParticipantService _participantService;
        private readonly IMeetingInfoService _meetingInfoService;
        private readonly IDailyService _dailyService;
        private readonly INavigationService _navigationService;
        private readonly ITimeStampProvider _timeStampProvider;
        private readonly IMainThreadInvoker _mainThreadInvoker;

        private Timer? _timer;
        private DailyState _state = new DailyState();
        private Uri? _sprintBoardUri;

        public DailyViewModel(
            DailyState state,
            IParticipantService participantService,
            IMeetingInfoService meetingInfoService,
            IDailyService dailyService,
            INavigationService navigationService,
            ITimeStampProvider timeStampProvider,
            IMainThreadInvoker mainThreadInvoker)
        {
            State = state ?? throw new ArgumentNullException(nameof(state));
            _participantService = participantService ?? throw new ArgumentNullException(nameof(participantService));
            _meetingInfoService = meetingInfoService ?? throw new ArgumentNullException(nameof(meetingInfoService));
            _dailyService = dailyService ?? throw new ArgumentNullException(nameof(dailyService));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _timeStampProvider = timeStampProvider ?? throw new ArgumentNullException(nameof(timeStampProvider));
            _mainThreadInvoker = mainThreadInvoker ?? throw new ArgumentNullException(nameof(mainThreadInvoker));

            NavigateBackCommand = new AsyncRelayCommand(NavigateBackAsync);
            NextSpeakerCommand = new AsyncRelayCommand(SetNextSpeakerAsync);
        }

        public AsyncRelayCommand NavigateBackCommand { get; }

        public AsyncRelayCommand NextSpeakerCommand { get; }

        public DailyState State
        {
            get => _state;
            set => SetProperty(ref _state, value);
        }

        public Uri? SprintBoardUri
        {
            get => _sprintBoardUri;
            private set => SetProperty(ref _sprintBoardUri, value);
        }

        public string Time => _timeStampProvider.CurrentClock.ToString(@"hh\:mm\:ss");

        public double Progress => _dailyService.CalculateMeetingPercentage(State.MeetingInfo);

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public async Task LoadDataAsync()
        {
            await _meetingInfoService.LoadAsync(State);
            await _participantService.LoadParticipantsForMeetingAsync(State.MeetingInfo, State);
            SprintBoardUri = new Uri(State.MeetingInfo.SprintBoardUri);

            _timer = new Timer(OnTimerElapsed);
            _timer.Change(0, 100);
        }

        public Task OnNavigatedToAsync(NavigationMode navigationMode)
        {
            return Task.CompletedTask;
        }

        public Task<bool> OnNavigatingFromAsync(NavigationMode navigationMode)
        {
            return Task.FromResult(true);
        }

        private void OnTimerElapsed(object? state)
        {
            _mainThreadInvoker.InvokeAsync(() =>
            {
                OnPropertyChanged(nameof(Time));
                OnPropertyChanged(nameof(Progress));
                _dailyService.RefreshParticipantsAsync(State);
            });
        }

        private Task SetNextSpeakerAsync()
        {
            return _dailyService.SetNextParticipantAsync(State);
        }

        private Task NavigateBackAsync()
        {
            return _navigationService.GoBackAsync();
        }
    }
}