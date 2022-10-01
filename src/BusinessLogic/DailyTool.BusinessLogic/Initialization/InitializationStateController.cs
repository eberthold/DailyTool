using DailyTool.BusinessLogic.Peoples;
using System.Collections.ObjectModel;

namespace DailyTool.BusinessLogic.Initialization
{
    public class InitializationStateController : IInitializationStateController
    {
        private readonly IInitializationService _initializationService;
        private InitializationStageState? _stageState;

        public InitializationStateController(
            IInitializationService initializationService)
        {
            _initializationService = initializationService;
        }

        public Task AddPersonAsync(Person person)
        {
            throw new NotImplementedException();
        }

        public async Task<InitializationStageState> GetStateAsync()
        {
            if (_stageState is null)
            {
                var people = await _initializationService.GetPeopleAsync();
                var meetingInfo = await _initializationService.GetMeetingInfoAsync();
                _stageState = new InitializationStageState
                {
                    People = new ObservableCollection<Person>(people),
                    MeetingInfo = meetingInfo
                };
            }

            return _stageState;
        }

        public Task RemovePersonAsync(Person person)
        {
            throw new NotImplementedException();
        }

        public Task SaveStateAsync()
        {
            throw new NotImplementedException();
        }
    }
}
