using CommunityToolkit.Mvvm.ComponentModel;
using DailyTool.ViewModels.Navigation;
using DailyTool.ViewModels.Settings;
using Scrummy.Core.BusinessLogic.Teams;
using Scrummy.ViewModels.Shared.Data;
using System.Collections.ObjectModel;

namespace DailyTool.ViewModels.Teams
{
    public class EditTeamViewModel : ObservableObject, INavigationTarget, ILoadDataAsync
    {
        private readonly ITeamService _teamService;

        public EditTeamViewModel(ITeamService teamService)
        {
            _teamService = teamService;
        }

        public ObservableCollection<TeamMemberViewModel> TeamMembers = new ObservableCollection<TeamMemberViewModel>();

        public async Task LoadDataAsync()
        {
            //var members = _teamService.GetByIdAsync();

        }

        public Task OnNavigatedToAsync(IReadOnlyDictionary<string, string> parameters, NavigationMode navigationMode)
            => Task.CompletedTask;

        public Task<bool> OnNavigatingFromAsync(NavigationMode navigationMode)
            => Task.FromResult(true);
    }
}
