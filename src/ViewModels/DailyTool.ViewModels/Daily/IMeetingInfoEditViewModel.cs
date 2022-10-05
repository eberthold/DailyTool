using DailyTool.ViewModels.Abstractions;
using DailyTool.ViewModels.Navigation;

namespace DailyTool.ViewModels.Daily
{
    public interface IMeetingInfoEditViewModel : INavigationTarget, ILoadDataAsync
    {
        TimeSpan EndTime { get; }

        TimeSpan StartTime { get; }

        string SprintBoardUri { get; }
    }
}
