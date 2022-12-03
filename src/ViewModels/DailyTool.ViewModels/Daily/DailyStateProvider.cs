using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;

namespace DailyTool.ViewModels.Daily
{
    public class DailyStateProvider : IDailyStateProvider
    {
        private static readonly ParticipantInitializationSettings ParticipantInitializationSettings = new()
        {
            Shuffle = true
        };

        private readonly IDailyService _dailyService;
        private DailyState _currentState = new DailyState();
        private bool _stateIsInitialized = false;

        public DailyStateProvider(IDailyService dailyService)
        {
            _dailyService = dailyService ?? throw new ArgumentNullException(nameof(dailyService));
        }

        private DailyState CurrentState
        {
            get => _currentState;
            set
            {
                _currentState = value;
                _stateIsInitialized = false;
            }
        }

        public async Task<DailyState> GetAsync()
        {
            if (!_stateIsInitialized)
            {
                await _dailyService.InitializeMeetingInfoAsync(_currentState).ConfigureAwait(false);
                await _dailyService.InitializeParticipantsAsync(_currentState, ParticipantInitializationSettings).ConfigureAwait(false);
                _stateIsInitialized = true;
            }

            return CurrentState;
        }

        public Task ResetState()
        {
            _currentState = new DailyState();
            return Task.CompletedTask;
        }
    }
}
