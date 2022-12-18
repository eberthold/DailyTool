using DailyTool.Infrastructure.Abstractions;
using Scrummy.Core.BusinessLogic.Teams;

namespace DailyTool.ViewModels.Teams
{
    public class TeamMapper : IMapper<TeamModel, TeamViewModel>, IMapper<TeamViewModel, TeamModel>
    {
        public TeamModel Map(TeamViewModel source)
        {
            var result = new TeamModel();
            Merge(source, result);
            return result;
        }

        public TeamViewModel Map(TeamModel source)
        {
            var result = new TeamViewModel();
            Merge(source, result);
            return result;
        }

        public void Merge(TeamViewModel source, TeamModel destination)
        {
            destination.Id = source.Id;
            destination.Name = source.Name;
        }

        public void Merge(TeamModel source, TeamViewModel destination)
        {
            destination.Id = source.Id;
            destination.Name = source.Name;
        }
    }
}
