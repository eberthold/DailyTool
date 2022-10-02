using DailyTool.BusinessLogic.Parameters;

namespace DailyTool.BusinessLogic.Daily
{
    public interface IDailyDataService
    {
        Task<IReadOnlyCollection<Participant>> GetParticipantsAsync();

        Task<TimeSpan> CalculateAverageTalkDuration(MeetingInfo meetingInfo, IReadOnlyCollection<Participant> participants);

        double CalculateMeetingPercentage(MeetingInfo meetingInfo);

        Task RefreshParticipantAsync(Participant participant);
    }
}
