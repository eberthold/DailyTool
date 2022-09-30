using DailyTool.BusinessLogic.Peoples;
using System.Collections.ObjectModel;

namespace DailyTool.BusinessLogic.Initialization
{
    public class InitializationStageState
    {
        public ObservableCollection<Person> Peoples { get; internal set; } = new();

        public TimeSpan MeetingDuration { get; internal set; }

        public TimeSpan MeetingStartTime { get; internal set; }
    }
}
