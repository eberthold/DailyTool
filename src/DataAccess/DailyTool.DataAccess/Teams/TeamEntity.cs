using DailyTool.DataAccess.People;
using DailyTool.Infrastructure.Abstractions.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyTool.DataAccess.Teams
{
    [Table("Teams")]
    public class TeamEntity : IIdentifiableSet
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }

        public List<PersonEntity> Members { get; set; } = new List<PersonEntity>();
    }
}
