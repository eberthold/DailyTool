namespace DailyTool.BusinessLogic.Daily
{
    public class MeetingInfo
    {
        public TimeSpan StartTime { get; set; }

        public TimeSpan Duration { get; set; }

        public string SprintBoardUri { get; set; } = string.Empty;
    }
}
