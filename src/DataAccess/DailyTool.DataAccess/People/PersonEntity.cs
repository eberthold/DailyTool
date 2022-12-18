using DailyTool.Infrastructure.Abstractions.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyTool.DataAccess.People
{
    [Table("People")]
    public class PersonEntity : IIdentifiableSet
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? EMailAddress { get; set; }
    }
}
