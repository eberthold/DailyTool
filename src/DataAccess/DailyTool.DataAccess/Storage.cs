namespace DailyTool.DataAccess
{
    public class Storage
    {
        public IReadOnlyCollection<PersonStorage> Peoples { get; set; } = Array.Empty<PersonStorage>();

        public TimeSpan MeetingDuration { get; set; }

        public TimeSpan MeetingStartTime { get; set; }
    }
}
