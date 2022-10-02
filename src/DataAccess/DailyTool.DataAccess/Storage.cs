namespace DailyTool.DataAccess
{
    public class Storage
    {
        public IReadOnlyCollection<PersonStorage> People { get; set; } = Array.Empty<PersonStorage>();

        public TimeSpan MeetingStartTime { get; set; } = new TimeSpan(9, 0, 0);

        public TimeSpan MeetingDuration { get; set; } = new TimeSpan(0, 15, 0);

        public string SprintBoardUri { get; set; } = string.Empty;
    }
}
