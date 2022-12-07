using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyTool.Infrastructure.Abstractions;
using DailyTool.ViewModels.Notifications;
using Scrummy.Core.BusinessLogic.Teams;

namespace Scrummy.Core.ViewModels.Teams
{
    public class TeamViewModel : ObservableObject
    {
        private readonly ITeamService _teamService;
        private readonly IMapper<TeamViewModel, TeamModel> _modelMapper;
        private readonly IMapper<TeamModel, TeamViewModel> _viewModelMapper;
        private readonly INotificationService _notificationService;
        private string _name = string.Empty;
        private int _id;

        public TeamViewModel(
            ITeamService teamService,
            IMapper<TeamViewModel, TeamModel> modelMapper,
            IMapper<TeamModel, TeamViewModel> viewModelMapper,
            INotificationService notificationService)
        {
            _teamService = teamService;
            _modelMapper = modelMapper;
            _viewModelMapper = viewModelMapper;
            _notificationService = notificationService;

            AddTeamCommand = new AsyncRelayCommand(AddTeamAsync);
            UpdateAsync = new AsyncRelayCommand(UpdateTeamAsync);
            DeleteTeamCommand = new AsyncRelayCommand(DeleteTeamAsync);
        }

        public AsyncRelayCommand AddTeamCommand { get; }

        public AsyncRelayCommand UpdateAsync { get; }

        public AsyncRelayCommand DeleteTeamCommand { get; }

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private async Task AddTeamAsync()
        {
            var model = _modelMapper.Map(this);
            Id = await _teamService.CreateAsync(model);
        }

        private async Task UpdateTeamAsync()
        {
            var model = _modelMapper.Map(this);
            await _teamService.UpdateAsync(model);
        }

        private async Task DeleteTeamAsync()
        {
            await _teamService.DeleteAsync(Id);
        }
    }
}
