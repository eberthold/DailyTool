namespace DailyTool.DataAccess
{
    public class Storage
    {
        public List<string> Peoples { get; set; } = new List<string>();

        public TimeSpan MeetingDuration { get; set; }

        public TimeSpan MeetingStartTime { get; set; }
    }
}
