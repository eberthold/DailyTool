namespace DailyTool.BusinessLogic.Daily.Abstractions
{
    public interface IDailyService
    {
        Task ShuffleParticipantsAsync(DailyState state);

        Task InitializeParticipantsAsync(DailyState state, ParticipantInitializationSettings settings);

        Task InitializeMeetingInfoAsync(DailyState state);

        Task RefreshStateAsync(DailyState state);

        Task SetPreviousParticipantAsync(IReadOnlyCollection<ParticipantModel> participants);

        Task SetNextParticipantAsync(IReadOnlyCollection<ParticipantModel> participants);
    }
}
