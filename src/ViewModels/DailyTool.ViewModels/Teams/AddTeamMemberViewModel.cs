using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyTool.Infrastructure.Abstractions;
using Scrummy.Core.BusinessLogic.Teams;
using Scrummy.Core.ViewModels.Navigation;

namespace DailyTool.ViewModels.Teams
{
    public class AddTeamMemberViewModel : ObservableObject, INavigationTarget
    {
        private readonly ITeamService _teamService;
        private readonly IMapper<TeamViewModel, TeamModel> _mapper;
        private readonly INavigationService _navigationService;
        private TeamViewModel _team = new TeamViewModel();

        public AddTeamMemberViewModel(
            ITeamService teamService,
            IMapper<TeamViewModel, TeamModel> mapper,
            INavigationService navigationService)
        {
            _teamService = teamService;
            _mapper = mapper;
            _navigationService = navigationService;

            SaveCommand = new AsyncRelayCommand(SaveAsync);
        }

        public IRelayCommand SaveCommand { get; }

        public TeamViewModel Team
        {
            get => _team;
            set => SetProperty(ref _team, value);
        }

        public Task OnNavigatedToAsync(IReadOnlyDictionary<string, string> parameters, NavigationMode navigationMode)
            => Task.CompletedTask;

        public Task<bool> OnNavigatingFromAsync(NavigationMode navigationMode)
            => Task.FromResult(true);

        private async Task SaveAsync()
        {
            var model = _mapper.Map(Team);
            await _teamService.CreateAsync(model);
            await _navigationService.NavigateBackAsync();
        }
    }
}