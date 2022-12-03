namespace DailyTool.BusinessLogic.Daily.Abstractions
{
    public interface IParticipantService
    {
        Task<IReadOnlyCollection<Participant>> GetAllAsync();

        Task RefreshParticipantsAsync(IReadOnlyCollection<Participant> participants, MeetingInfo meetingInfo);

        IReadOnlyCollection<Participant> ShuffleQueuedParticipants(IReadOnlyCollection<Participant> participants);
    }
}
