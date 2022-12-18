namespace DailyTool.BusinessLogic.Daily.Abstractions
{
    public interface IParticipantService
    {
        Task<IReadOnlyCollection<ParticipantModel>> GetParticipantsAsync(int meetingId);

        Task RefreshParticipantsAsync(IReadOnlyCollection<ParticipantModel> participants, DailyMeetingModel meetingInfo);

        IReadOnlyCollection<ParticipantModel> ShuffleQueuedParticipants(IReadOnlyCollection<ParticipantModel> participants);
    }
}
