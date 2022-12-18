using DailyTool.Infrastructure.Abstractions.Data;

namespace DailyTool.BusinessLogic.Daily
{
    public class PersonModel : IIdentifiable
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string EMailAddress { get; set; } = string.Empty;
    }
}