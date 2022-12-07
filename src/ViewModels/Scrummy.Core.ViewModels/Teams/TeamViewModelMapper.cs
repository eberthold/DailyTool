using DailyTool.Infrastructure.Abstractions;
using DailyTool.ViewModels.Notifications;
using Scrummy.Core.BusinessLogic.Teams;

namespace Scrummy.Core.ViewModels.Teams
{
    public class TeamViewModelMapper : IMapper<TeamModel, TeamViewModel>, IMapper<TeamViewModel, TeamModel>
    {
        private readonly ITeamService _teamService;
        private readonly INotificationService _notificationService;

        public TeamViewModelMapper(
            ITeamService teamService,
            INotificationService notificationService)
        {
            _teamService = teamService;
            _notificationService = notificationService;
        }

        public TeamModel Map(TeamViewModel source)
        {
            var result = new TeamModel();
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


        TeamViewModel IMapper<TeamModel, TeamViewModel>.Map(TeamModel source)
        {
            var result = new TeamViewModel(
                _teamService,
                this,
                this,
                _notificationService);

            Merge(source, result);
            return result;
        }
    }
}
