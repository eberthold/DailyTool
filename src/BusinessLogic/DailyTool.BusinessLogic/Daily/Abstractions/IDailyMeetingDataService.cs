namespace DailyTool.BusinessLogic.Daily.Abstractions
{
    public interface IDailyMeetingDataService
    {
        Task<DailyMeetingModel> GetByIdAsync(int meetingId);

        Task<DailyMeetingModel> GetByTeamAsync(int teamId);

        Task UpdateAsync(DailyMeetingModel meetingInfo);

        double CalculateMeetingPercentage(DailyMeetingModel meetingInfo);
    }
}
