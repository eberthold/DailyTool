using DailyTool.ViewModels.Abstractions;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DailyTool.ViewModels.Daily
{
    public interface IPeopleState : INotifyPropertyChanged, ILoadDataAsync
    {
        ObservableCollection<PersonViewModel> People { get; }
    }
}
