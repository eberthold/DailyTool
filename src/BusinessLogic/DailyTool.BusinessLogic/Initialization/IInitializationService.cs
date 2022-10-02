using DailyTool.BusinessLogic.Parameters;
using DailyTool.BusinessLogic.People;

namespace DailyTool.BusinessLogic.Initialization
{
    public interface IInitializationService
    {
        Task<MeetingInfo> GetMeetingInfoAsync();

        Task<IReadOnlyCollection<Person>> GetPeopleAsync();

        Task SaveMeetingInfoAsync(MeetingInfo meetingInfo);

        Task SavePeopleAsync(IReadOnlyCollection<Person> people);
    }
}
