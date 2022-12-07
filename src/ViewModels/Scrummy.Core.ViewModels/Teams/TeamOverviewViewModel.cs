using DailyTool.Infrastructure.Abstractions;
using Scrummy.Core.BusinessLogic.Teams;
using Scrummy.ViewModels.Shared.Data;
using System.Collections.ObjectModel;

namespace Scrummy.Core.ViewModels.Teams
{
    public class TeamOverviewViewModel : ILoadDataAsync
    {
        private readonly ITeamService _teamService;
        private readonly IMapper<TeamModel, TeamViewModel> _viewModelMapper;

        public TeamOverviewViewModel(
            ITeamService teamService,
            IMapper<TeamModel, TeamViewModel> viewModelMapper)
        {
            _teamService = teamService;
            _viewModelMapper = viewModelMapper;
        }

        public ObservableCollection<TeamViewModel> Teams { get; set; } = new ObservableCollection<TeamViewModel>();

        public async Task LoadDataAsync()
        {
            var teams = await _teamService.GetAllAsync();
            Teams = new ObservableCollection<TeamViewModel>(teams.Select(_viewModelMapper.Map));
        }
    }
}
