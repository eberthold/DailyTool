namespace DailyTool.BusinessLogic.Daily
{
    public record MeetingInfo
    {
        public TimeSpan MeetingDuration { get; init; }

        public TimeSpan MeetingStartTime { get; init; }

        public string SprintBoardUri { get; init; } = string.Empty;
    }
}
