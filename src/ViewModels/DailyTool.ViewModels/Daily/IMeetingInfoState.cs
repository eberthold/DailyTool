using DailyTool.ViewModels.Abstractions;
using System.ComponentModel;

namespace DailyTool.ViewModels.Daily
{
    public interface IMeetingInfoState : INotifyPropertyChanged, ILoadDataAsync
    {
        TimeSpan StartTime { get; set; }

        TimeSpan Duration { get; set; }

        TimeSpan EndTime { get; set; }

        string SprintBoardUri { get; set; }
    }
}
