namespace DailyTool.BusinessLogic.Daily.Abstractions
{
    public interface IMeetingInfoRepository
    {
        Task<MeetingInfo> GetAsync();

        Task SaveAsync(MeetingInfo meetingInfo);
    }
}
