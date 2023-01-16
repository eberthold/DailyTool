using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DailyTool.ViewModels.Abstractions
{
    public interface IOverviewViewModel<T> : INotifyPropertyChanged
        where T : INotifyPropertyChanged
    {
        ObservableCollection<T> Items { get; set; }

        T? SelectedItem { get; set; }
    }
}
