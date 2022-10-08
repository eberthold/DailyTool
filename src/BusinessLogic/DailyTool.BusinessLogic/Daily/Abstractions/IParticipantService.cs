namespace DailyTool.BusinessLogic.Daily.Abstractions
{
    public interface IParticipantService
    {
        Task<IReadOnlyCollection<Participant>> GetAllAsync();

        Task RefreshParticipantsAsync(IReadOnlyCollection<Participant> participants);

        void ShuffleParticipantsIndex(IReadOnlyCollection<Participant> participants);

        void CalculateAllocatedTimeSlots(IReadOnlyCollection<Participant> participants, MeetingInfo meetingInfo);

        Task SetPreviousParticipantAsync(IReadOnlyCollection<Participant> participants);

        Task SetNextParticipantAsync(IReadOnlyCollection<Participant> participants);
    }
}
