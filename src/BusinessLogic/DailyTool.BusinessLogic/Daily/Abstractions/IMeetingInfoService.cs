namespace DailyTool.BusinessLogic.Daily.Abstractions
{
    public interface IMeetingInfoService
    {
        Task<MeetingInfo> GetAsync();

        Task UpdateAsync(MeetingInfo meetingInfo);

        double CalculateMeetingPercentage(MeetingInfo meetingInfo);
    }
}
