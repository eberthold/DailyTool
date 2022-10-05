using DailyTool.ViewModels.Abstractions;
using DailyTool.ViewModels.Navigation;
using System.ComponentModel;

namespace DailyTool.ViewModels.Daily
{
    public interface IPeopleEditViewModel : INotifyPropertyChanged, ILoadDataAsync, INavigationTarget
    {
        bool IsInViewMode { get; }

        bool IsInPersonAddMode { get; }
    }
}
