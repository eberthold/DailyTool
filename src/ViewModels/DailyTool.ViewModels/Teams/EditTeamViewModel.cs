using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyTool.Infrastructure.Abstractions;
using DailyTool.ViewModels.Abstractions;
using DailyTool.ViewModels.Notifications;
using Scrummy.Core.BusinessLogic.Teams;
using Scrummy.Core.ViewModels.Navigation;
using Scrummy.Core.ViewModels.Parameters;
using Scrummy.ViewModels.Shared.Data;

namespace DailyTool.ViewModels.Teams
{
    public class EditTeamViewModel : ObservableObject, INavigationTarget<TeamParameter>, ILoadDataAsync, ISaveDataAsync, INotifyClose
    {
        internal const string IdParameter = "Id";

        private readonly ITeamService _teamService;
        private readonly INotificationService _notificationService;
        private readonly IMapper<TeamViewModel, TeamModel> _modelMapper;
        private readonly IMapper<TeamModel, TeamViewModel> _viewModelMapper;

        private int _teamId = 0;
        private TeamViewModel _team = new();

        public EditTeamViewModel(
            ITeamService teamService,
            INotificationService notificationService,
            IMapper<TeamViewModel, TeamModel> modelMapper,
            IMapper<TeamModel, TeamViewModel> viewModelMapper)
        {
            _teamService = teamService;
            _notificationService = notificationService;
            _modelMapper = modelMapper;
            _viewModelMapper = viewModelMapper;

            SaveCommand = new AsyncRelayCommand(SaveDataAsync);
            CancelCommand = new AsyncRelayCommand(CancelAsync);
        }

        public event EventHandler? Closed;

        public IRelayCommand SaveCommand { get; }

        public IRelayCommand CancelCommand { get; }

        public TeamViewModel Team
        {
            get => _team;
            set => SetProperty(ref _team, value);
        }

        public async Task LoadDataAsync()
        {
            if (_teamId is 0)
            {
                return;
            }

            var teamModel = await _teamService.GetAsync(_teamId);
            Team = _viewModelMapper.Map(teamModel);
        }

        public async Task OnNavigatedToAsync(NavigationMode navigationMode)
        {
            await LoadDataAsync();
        }

        public Task<bool> OnNavigatingFromAsync(NavigationMode navigationMode)
        {
            return Task.FromResult(true);
        }

        public async Task SaveDataAsync()
        {
            try
            {
                var team = _modelMapper.Map(Team);

                if (team.Id > 0)
                {
                    await _teamService.UpdateAsync(team);
                }
                else
                {
                    await _teamService.CreateAsync(team);
                }

                Closed?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                var notification = new Notification
                {
                    Text = $"{ex.Message}{Environment.NewLine}{ex.StackTrace}",
                    NotificationType = NotificationType.Error
                };

                await _notificationService.ShowNotificationAsync(notification);
            }
        }

        private Task CancelAsync()
        {
            Closed?.Invoke(this, EventArgs.Empty);
            return Task.CompletedTask;
        }

        public void SetParameters(TeamParameter parameter)
        {
            _teamId = parameter.TeamId;
        }
    }
}