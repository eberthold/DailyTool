using System.Collections.ObjectModel;

namespace DailyTool.BusinessLogic.Daily
{
    public class DailyState
    {
        public ObservableCollection<Participant> Participants { get; internal set; } = new ObservableCollection<Participant>();

        public string SprintBoardUri { get; internal set; } = string.Empty;
    }
}
