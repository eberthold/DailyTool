using DailyTool.ViewModels.Abstractions;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DailyTool.ViewModels.Daily
{
    public interface IParticipantState : INotifyPropertyChanged, ILoadDataAsync
    {
        ObservableCollection<ParticipantViewModel> Participants { get; }
    }
}