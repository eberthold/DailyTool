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
        private readonly ITimeStampProvider _timeStampProvider;
        private readonly IMainThreadInvoker _mainThreadInvoker;

        private Timer? _timer;
        private DailyState _currentState = new DailyState();

        public DailyViewModel(
            IDailyStateService stateService,
            ITimeStampProvider timeStampProvider,
            IMainThreadInvoker mainThreadInvoker)
        {
            _stateService = stateService ?? throw new ArgumentNullException(nameof(stateService));
            _timeStampProvider = timeStampProvider ?? throw new ArgumentNullException(nameof(timeStampProvider));
            _mainThreadInvoker = mainThreadInvoker ?? throw new ArgumentNullException(nameof(mainThreadInvoker));

            NextSpeakerCommand = new AsyncRelayCommand(SetNextSpeakerAsync);
        }

        public AsyncRelayCommand NextSpeakerCommand { get; }

        public DailyState CurrentState
        {
            get => _currentState;
            set => SetProperty(ref _currentState, value);
        }

        public string Time => _timeStampProvider.CurrentClock.ToString(@"hh\:mm\:ss");

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public async Task LoadDataAsync()
        {
            CurrentState = await _stateService.GetDailyStateAsync();

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
            });
        }

        private Task SetNextSpeakerAsync()
        {
            return _stateService.SetNextParticipantAsync();
        }
    }
}