using DailyTool.DataAccess.Teams;
using DailyTool.Infrastructure.Abstractions.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyTool.DataAccess.Meetings
{
    [Table("Dailies")]
    public class DailyMeetingEntity : IIdentifiableSet
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Team))]
        public int TeamId { get; set; }

        public TeamEntity? Team { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan Duration { get; set; }

        public string? SprintBoardUri { get; set; }
    }
}
