using DailyTool.Infrastructure.Abstractions.Data;

namespace DailyTool.BusinessLogic.Daily
{
    public class DailyMeetingModel : IIdentifiable
    {
        public int Id { get; set; }

        public int TeamId { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan Duration { get; set; }

        public string SprintBoardUri { get; set; } = string.Empty;
    }
}
