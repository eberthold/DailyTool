namespace DailyTool.BusinessLogic.Daily.Abstractions
{
    public interface IMeetingParticipantsRepository
    {
        Task<int> AddParticipantAsync(int dailyMeetingId, int personId);

        Task<IReadOnlyCollection<ParticipantModel>> GetParticipantsAsync(int dailyMeetingId);

        Task RemoveParticipantAsync(int participantId);
    }
}
