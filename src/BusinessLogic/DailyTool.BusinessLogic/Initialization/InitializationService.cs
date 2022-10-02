using DailyTool.BusinessLogic.Parameters;
using DailyTool.BusinessLogic.People;

namespace DailyTool.BusinessLogic.Initialization
{
    public class InitializationService : IInitializationService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IMeetingInfoRepository _meetingInfoRepository;

        public InitializationService(
            IPersonRepository personRepository,
            IMeetingInfoRepository meetingInfoRepository)
        {
            _personRepository = personRepository ?? throw new ArgumentNullException(nameof(personRepository));
            _meetingInfoRepository = meetingInfoRepository;
        }

        public Task<IReadOnlyCollection<Person>> GetPeopleAsync()
        {
            return _personRepository.GetAllAsync();
        }

        public Task SavePeopleAsync(IReadOnlyCollection<Person> people)
        {
            return _personRepository.SaveAllAsync(people);
        }

        public Task<MeetingInfo> GetMeetingInfoAsync()
        {
            return _meetingInfoRepository.GetAsync();
        }

        public Task SaveMeetingInfoAsync(MeetingInfo meetingInfo)
        {
            return _meetingInfoRepository.SaveAsync(meetingInfo);
        }
    }
}
