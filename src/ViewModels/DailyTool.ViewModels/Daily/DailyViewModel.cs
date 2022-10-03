using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.System;
using DailyTool.ViewModels.Abstractions;
using DailyTool.ViewModels.Navigation;

namespace DailyTool.ViewModels.Daily
{
    public class DailyViewModel : ObservableObject, INavigationTarget, ILoadDataAsync, IDisposable
    {
        private readonly IDailyStateService _stateService;
        private readonly IDailyDataService _dataService;
        private readonly INavigationService _navigationService;
        private readonly ITimeStampProvider _timeStampProvider;
        private readonly IMainThreadInvoker _mainThreadInvoker;

        private Timer? _timer;
        private DailyState _currentState = new DailyState();

        public DailyViewModel(
            IDailyStateService stateService,
            IDailyDataService dataService,
            INavigationService navigationService,
            ITimeStampProvider timeStampProvider,
            IMainThreadInvoker mainThreadInvoker)
        {
            _stateService = stateService ?? throw new ArgumentNullException(nameof(stateService));
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _timeStampProvider = timeStampProvider ?? throw new ArgumentNullException(nameof(timeStampProvider));
            _mainThreadInvoker = mainThreadInvoker ?? throw new ArgumentNullException(nameof(mainThreadInvoker));

            NavigateBackCommand = new AsyncRelayCommand(NavigateBackAsync);
            NextSpeakerCommand = new AsyncRelayCommand(SetNextSpeakerAsync);
        }

        public AsyncRelayCommand NavigateBackCommand { get; }

        public AsyncRelayCommand NextSpeakerCommand { get; }

        public DailyState CurrentState
        {
            get => _currentState;
            set => SetProperty(ref _currentState, value);
        }

        public string Time => _timeStampProvider.CurrentClock.ToString(@"hh\:mm\:ss");

        public double Progress => _dataService.CalculateMeetingPercentage(CurrentState.MeetingInfo);

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public async Task LoadDataAsync()
        {
            CurrentState = await _stateService.GetDailyStateAsync(true);

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
            });
        }

        private Task SetNextSpeakerAsync()
        {
            return _stateService.SetNextParticipantAsync();
        }

        private Task NavigateBackAsync()
        {
            return _navigationService.GoBackAsync();
        }
    }
}