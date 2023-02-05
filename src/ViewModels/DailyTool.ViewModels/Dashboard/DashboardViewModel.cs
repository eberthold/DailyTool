using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyTool.ViewModels.Abstractions;
using DailyTool.ViewModels.Data;
using DailyTool.ViewModels.Initialization;
using DailyTool.ViewModels.Teams;
using Scrummy.Core.BusinessLogic.Teams;
using Scrummy.Core.ViewModels.Navigation;
using Scrummy.Core.ViewModels.Parameters;
using Scrummy.ViewModels.Shared.Data;
using System.Collections.ObjectModel;

namespace DailyTool.ViewModels.Dashboard
{
    [NavigationTarget("/dashboard")]
    public class DashboardViewModel : ObservableObject, IOverviewViewModel<TeamViewModel>, INavigationTarget, ILoadDataAsync, ITitle
    {
        private readonly IOverviewViewModelService<TeamViewModel> _overviewViewModelService;
        private readonly INavigationService _navigationService;
        private readonly ITeamContext _teamContext;

        private ObservableCollection<TeamViewModel> _items = new ObservableCollection<TeamViewModel>();

        public DashboardViewModel(
            IOverviewViewModelService<TeamViewModel> overviewViewModelService,
            INavigationService navigationService,
            ITeamContext teamContext)
        {
            _overviewViewModelService = overviewViewModelService;
            _navigationService = navigationService;
            _teamContext = teamContext;

            NavigateToDailyCommand = new AsyncRelayCommand(NavigateToDailyAsync, CanNavigateToDaily);
        }

        public IRelayCommand NavigateToDailyCommand { get; private set; }

        public ObservableCollection<TeamViewModel> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        public TeamViewModel? SelectedItem
        {
            get => Items.FirstOrDefault(x => x.Id == _teamContext.CurrentTeamId);
            set
            {
                _teamContext.CurrentTeamId = value?.Id ?? 0;
                OnPropertyChanged();
            }
        }

        public object Title => this;

        public async Task LoadDataAsync()
        {
            await _overviewViewModelService.LoadDataAsync(this);

            // TODO: team context must retrieve initial value from storage.
            _teamContext.CurrentTeamId = Items.FirstOrDefault()?.Id ?? 0;
            OnPropertyChanged(nameof(SelectedItem));
        }

        public async Task OnNavigatedToAsync(NavigationMode navigationMode)
        {
            await LoadDataAsync();
        }

        public Task<bool> OnNavigatingFromAsync(NavigationMode navigationMode)
        {
            return Task.FromResult(true);
        }

        private bool CanNavigateToDaily()
        {
            return SelectedItem is not null;
        }

        private Task NavigateToDailyAsync()
        {
            if (SelectedItem is null)
            {
                return Task.CompletedTask;
            }

            return _navigationService.NavigateAsync<InitializationViewModel, TeamParameter>(new TeamParameter
            {
                TeamId = SelectedItem?.Id ?? 0,
            });
        }
    }
}
