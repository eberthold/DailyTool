using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.BusinessLogic.System;
using DailyTool.ViewModels.Abstractions;
using DailyTool.ViewModels.Navigation;

namespace DailyTool.ViewModels.Daily
{
    public class DailyViewModel : ObservableObject, INavigationTarget, ILoadDataAsync, IDisposable
    {
        private readonly IParticipantService _participantService;
        private readonly INavigationService _navigationService;
        private readonly ITimeStampProvider _timeStampProvider;
        private readonly IMainThreadInvoker _mainThreadInvoker;

        private Timer? _timer;

        public DailyViewModel(
            IMeetingInfoState meetingInfoState,
            IParticipantState participantState,
            IParticipantService participantService,
            INavigationService navigationService,
            ITimeStampProvider timeStampProvider,
            IMainThreadInvoker mainThreadInvoker)
        {
            MeetingInfoState = meetingInfoState ?? throw new ArgumentNullException(nameof(meetingInfoState));
            ParticipantState = participantState ?? throw new ArgumentNullException(nameof(participantState));
            _participantService = participantService ?? throw new ArgumentNullException(nameof(participantService));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _timeStampProvider = timeStampProvider ?? throw new ArgumentNullException(nameof(timeStampProvider));
            _mainThreadInvoker = mainThreadInvoker ?? throw new ArgumentNullException(nameof(mainThreadInvoker));

            NavigateBackCommand = new AsyncRelayCommand(NavigateBackAsync);
            PreviousSpeakerCommand = new AsyncRelayCommand(SetPreviousParticipantAsync);
            NextSpeakerCommand = new AsyncRelayCommand(SetNextParticipantAsync);
        }

        public AsyncRelayCommand NavigateBackCommand { get; }

        public AsyncRelayCommand PreviousSpeakerCommand { get; }

        public AsyncRelayCommand NextSpeakerCommand { get; }

        public IMeetingInfoState MeetingInfoState { get; }

        public IParticipantState ParticipantState { get; }

        public string Time => _timeStampProvider.CurrentClock.ToString(@"hh\:mm\:ss");

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public async Task LoadDataAsync()
        {
            await ParticipantState.LoadDataAsync();
            await MeetingInfoState.LoadDataAsync();

            _timer = new Timer(OnTimerElapsed);
            _timer.Change(0, 100);
        }

        public Task OnNavigatedToAsync(NavigationMode navigationMode)
         => Task.CompletedTask;

        public Task<bool> OnNavigatingFromAsync(NavigationMode navigationMode)
         => Task.FromResult(true);

        private void OnTimerElapsed(object? state)
        {
            _mainThreadInvoker.InvokeAsync(() =>
            {
                OnPropertyChanged(nameof(Time));
                ParticipantState.Refresh();
            });
        }

        private Task NavigateBackAsync()
            => _navigationService.GoBackAsync();

        private Task SetPreviousParticipantAsync()
            => ParticipantState.SetPreviousParticipant();

        private Task SetNextParticipantAsync()
            => ParticipantState.SetNextParticipant();
    }
}