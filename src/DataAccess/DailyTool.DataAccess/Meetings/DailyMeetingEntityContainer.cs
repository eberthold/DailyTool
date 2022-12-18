using DailyTool.Infrastructure.Abstractions.Data;

namespace DailyTool.DataAccess.Meetings
{
    public class DailyMeetingEntityContainer : IIdentifiableSet
    {
        public int Id
        {
            get => Meeting.Id;
            set => Meeting.Id = value;
        }

        public MeetingEntity Meeting { get; set; } = new();

        public string? SprintBoardUri { get; set; }
    }
}
