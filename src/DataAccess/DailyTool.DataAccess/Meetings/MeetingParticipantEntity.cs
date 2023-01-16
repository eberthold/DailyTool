using DailyTool.DataAccess.People;
using DailyTool.Infrastructure.Abstractions.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyTool.DataAccess.Meetings
{
    [Table("DailyMeetingParticipants")]
    public class MeetingParticipantEntity : IIdentifiableSet
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(DailyMeeting))]
        public int DailyMeetingId { get; set; }

        public DailyMeetingEntity DailyMeeting { get; set; } = new DailyMeetingEntity();

        [ForeignKey(nameof(Person))]
        public int PersonId { get; set; }

        public PersonEntity Person { get; set; } = new PersonEntity();
    }
}
