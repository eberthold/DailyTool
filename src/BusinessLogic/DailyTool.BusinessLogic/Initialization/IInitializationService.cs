using DailyTool.BusinessLogic.Parameters;
using DailyTool.BusinessLogic.Peoples;

namespace DailyTool.BusinessLogic.Initialization
{
    public interface IInitializationService
    {
        Task<MeetingInfo> GetMeetingInfoAsync();

        Task<IReadOnlyCollection<Person>> GetPeopleAsync();

        Task SaveMeetingInfoAsync(MeetingInfo meetingInfo);

        Task SavePeoplesAsync(IReadOnlyCollection<Person> peoples);
    }
}
