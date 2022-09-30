using DailyTool.BusinessLogic.Parameters;
using DailyTool.BusinessLogic.Peoples;

namespace DailyTool.BusinessLogic.Initialization
{
    public interface IInitializationStateController
    {
        Task<InitializationStageState> GetStateAsync();

        Task AddPerson(Person person);

        Task RemovePerson(Person person);

        Task UpdateMeetingInfoAsync(MeetingInfo meetingInfo);
    }
}
