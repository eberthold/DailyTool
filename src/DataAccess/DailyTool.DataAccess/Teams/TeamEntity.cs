using System.ComponentModel.DataAnnotations;

namespace DailyTool.DataAccess.Teams
{
    public class TeamEntity
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }
    }
}
