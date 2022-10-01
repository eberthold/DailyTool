namespace DailyTool.BusinessLogic.Parameters
{
    public interface IMeetingInfoRepository
    {
        Task<MeetingInfo> GetAsync();

        Task SaveAsync(MeetingInfo meetingInfo);
    }
}
