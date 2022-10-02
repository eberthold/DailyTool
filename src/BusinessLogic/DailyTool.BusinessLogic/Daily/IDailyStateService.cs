namespace DailyTool.BusinessLogic.Daily
{
    public interface IDailyStateService
    {
        Task<DailyState> GetDailyStateAsync();

        Task RefreshStateAsync();

        Task SetNextParticipantAsync();
    }
}
