namespace DailyTool.BusinessLogic.Daily
{
    public interface IDailyStateService
    {
        Task<DailyState> GetDailyStateAsync(bool force = false);

        Task RefreshStateAsync();

        Task SetNextParticipantAsync();
    }
}
