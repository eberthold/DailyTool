namespace DailyTool.BusinessLogic.Daily.Abstractions
{
    public interface IDailyMeetingDataService
    {
        Task<DailyMeetingModel> GetAsync(int meetingId);

        Task UpdateAsync(DailyMeetingModel meetingInfo);

        double CalculateMeetingPercentage(DailyMeetingModel meetingInfo);
    }
}
