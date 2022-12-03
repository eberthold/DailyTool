using DailyTool.BusinessLogic.Daily;

namespace DailyTool.ViewModels.Daily
{
    public interface IDailyStateProvider
    {
        Task<DailyState> GetAsync();

        Task ResetState();
    }
}
