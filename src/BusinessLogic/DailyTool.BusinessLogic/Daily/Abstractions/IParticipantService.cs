namespace DailyTool.BusinessLogic.Daily.Abstractions
{
    public interface IParticipantService
    {
        Task LoadParticipantsForMeetingAsync(MeetingInfo meetingInfo, DailyState state);
    }
}
