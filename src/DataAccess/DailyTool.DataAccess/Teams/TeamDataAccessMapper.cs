using DailyTool.DataAccess.Helpers;
using DailyTool.Infrastructure.Abstractions;
using Scrummy.Core.BusinessLogic.Teams;

namespace DailyTool.DataAccess.Teams
{
    public class TeamDataAccessMapper : IMapper<TeamModel, TeamEntity>, IMapper<TeamEntity, TeamModel>
    {
        public TeamEntity Map(TeamModel source)
        {
            var result = new TeamEntity();
            Merge(source, result);
            return result;
        }

        public TeamModel Map(TeamEntity source)
        {
            var result = new TeamModel();
            Merge(source, result);
            return result;
        }

        public void Merge(TeamModel source, TeamEntity destination)
        {
            destination.Id = source.Id;
            destination.Name = source.Name.MapToEntity();
        }

        public void Merge(TeamEntity source, TeamModel destination)
        {
            destination.Id = source.Id;
            destination.Name = source.Name.MapToModel();
        }
    }
}
