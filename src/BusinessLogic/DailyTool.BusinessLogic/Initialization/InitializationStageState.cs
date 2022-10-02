using DailyTool.BusinessLogic.Parameters;
using DailyTool.BusinessLogic.People;
using System.Collections.ObjectModel;

namespace DailyTool.BusinessLogic.Initialization
{
    public class InitializationStageState
    {
        public ObservableCollection<Person> People { get; internal set; } = new();

        public MeetingInfo MeetingInfo { get; internal set; } = new();
    }
}
