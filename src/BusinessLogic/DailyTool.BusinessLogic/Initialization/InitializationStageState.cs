using DailyTool.BusinessLogic.Parameters;
using DailyTool.BusinessLogic.Peoples;
using System.Collections.ObjectModel;

namespace DailyTool.BusinessLogic.Initialization
{
    public class InitializationStageState
    {
        public ObservableCollection<Person> Peoples { get; internal set; } = new();

        public MeetingInfo MeetingInfo { get; internal set; } = new();
    }
}
