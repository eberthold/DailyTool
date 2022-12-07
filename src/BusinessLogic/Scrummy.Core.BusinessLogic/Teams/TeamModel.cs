using Scrummy.Core.BusinessLogic.Data;

namespace Scrummy.Core.BusinessLogic.Teams
{
    public class TeamModel : IIdentifiable
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
