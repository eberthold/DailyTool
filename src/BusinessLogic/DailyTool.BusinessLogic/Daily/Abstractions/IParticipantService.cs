namespace DailyTool.BusinessLogic.Daily.Abstractions
{
    public interface IParticipantService<T>
        where T : IParticipant
    {
        Task<IReadOnlyCollection<T>> GetAllAsync();

        Task RefreshParticipantsAsync(IReadOnlyCollection<T> participants);

        void ShuffleParticipantsIndex(IReadOnlyCollection<T> participants);

        void CalculateAllocatedTimeSlots(IReadOnlyCollection<T> participants, MeetingInfo meetingInfo);

        Task SetPreviousParticipantAsync(IReadOnlyCollection<T> participants);

        Task SetNextParticipantAsync(IReadOnlyCollection<T> participants);
    }
}
