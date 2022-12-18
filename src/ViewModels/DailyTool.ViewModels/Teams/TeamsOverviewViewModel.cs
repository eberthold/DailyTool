using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyTool.Infrastructure.Abstractions;
using DailyTool.ViewModels.Navigation;
using DailyTool.ViewModels.Settings;
using Scrummy.Core.BusinessLogic.Teams;
using Scrummy.ViewModels.Shared.Data;
using System.Collections.ObjectModel;

namespace DailyTool.ViewModels.Teams
{
    public class TeamsOverviewViewModel : ObservableObject, INavigationTarget, ILoadDataAsync, ISettingsViewModel
    {
        private readonly ITeamService _teamService;
        private readonly INavigationService _navigationService;
        private readonly IMapper<TeamModel, TeamViewModel> _viewModelMapper;

        private ObservableCollection<TeamViewModel> _teams = new ObservableCollection<TeamViewModel>();

        public TeamsOverviewViewModel(
            ITeamService teamService,
            INavigationService navigationService,
            IMapper<TeamModel, TeamViewModel> viewModelMapper)
        {
            _teamService = teamService;
            _navigationService = navigationService;
            _viewModelMapper = viewModelMapper;

            NavigateAddMemberCommand = new AsyncRelayCommand(NavigateTeamMemberAsync);
        }

        public AsyncRelayCommand NavigateAddMemberCommand { get; }

        public ObservableCollection<TeamViewModel> Teams
        {
            get => _teams;
            set => SetProperty(ref _teams, value);
        }

        public string Title => "TODO: Teams";

        public async Task LoadDataAsync()
        {
            var teams = await _teamService.GetAllAsync();
            var mappedTeams = teams.Select(_viewModelMapper.Map);

            Teams = new ObservableCollection<TeamViewModel>(mappedTeams);
        }

        private Task NavigateTeamMemberAsync()
        {
            return _navigationService.NavigateAsync<AddTeamMemberViewModel>();
        }

        public Task OnNavigatedToAsync(IReadOnlyDictionary<string, string> parameters, NavigationMode navigationMode)
            => Task.CompletedTask;

        public Task<bool> OnNavigatingFromAsync(NavigationMode navigationMode)
            => Task.FromResult(true);
    }
}
