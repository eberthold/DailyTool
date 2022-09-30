using DailyTool.BusinessLogic.Peoples;
using System.Collections.ObjectModel;

namespace DailyTool.BusinessLogic.Running
{
    internal class RunningStageState
    {
        private readonly ObservableCollection<Person> _participants = new();

        public RunningStageState()
        {
            Participants = new ReadOnlyObservableCollection<Person>(_participants);
        }

        public ReadOnlyObservableCollection<Person> Participants { get; }
    }
}