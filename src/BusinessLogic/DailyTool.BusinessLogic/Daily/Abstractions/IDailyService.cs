namespace DailyTool.BusinessLogic.Daily.Abstractions
{
    public interface IDailyService
    {
        double CalculateMeetingPercentage(MeetingInfo meetingInfo);

        Task RefreshParticipantsAsync(DailyState state);

        Task SetNextParticipantAsync(DailyState state);
    }
}
