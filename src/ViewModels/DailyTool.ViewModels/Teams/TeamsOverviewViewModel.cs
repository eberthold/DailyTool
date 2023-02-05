using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyTool.ViewModels.Abstractions;
using DailyTool.ViewModels.Data;
using DailyTool.ViewModels.Settings;
using Scrummy.Core.BusinessLogic.Teams;
using Scrummy.Core.ViewModels.Navigation;
using Scrummy.Core.ViewModels.Parameters;
using Scrummy.ViewModels.Shared.Data;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace DailyTool.ViewModels.Teams
{
    [NavigationTarget("/teams")]
    public class TeamsOverviewViewModel : ObservableObject, INavigationTarget, ILoadDataAsync, ISettingsViewModel, IOverviewViewModel<TeamViewModel>
    {
        private readonly ITeamService _teamService;
        private readonly IOverviewViewModelService<TeamViewModel> _overviewService;
        private readonly INavigationService _navigationService;

        private ObservableCollection<TeamViewModel> _items = new();
        private TeamViewModel? _selectedItem;
        private EditTeamViewModel? _editTeamViewModel;

        public TeamsOverviewViewModel(
            ITeamService teamService,
            IOverviewViewModelService<TeamViewModel> overviewService,
            INavigationService navigationService)
        {
            _teamService = teamService;
            _overviewService = overviewService;
            _navigationService = navigationService;

            AddTeamCommand = new AsyncRelayCommand(ShowAddTeamAsync);
            EditTeamCommand = new AsyncRelayCommand(ShowEditTeamAsync, CanEditTeam);
            DeleteTeamCommand = new AsyncRelayCommand(DeleteTeamAsync, CanDeleteTeam);
        }

        public IRelayCommand AddTeamCommand { get; }

        public IRelayCommand EditTeamCommand { get; }

        public IRelayCommand DeleteTeamCommand { get; }

        public ObservableCollection<TeamViewModel> Items
        {
            get => _items;
            set
            {
                _items.CollectionChanged -= OnItemsChanged;

                if (!SetProperty(ref _items, value))
                {
                    return;
                }

                _items.CollectionChanged += OnItemsChanged;
                OnPropertyChanged(nameof(HasTeams));
            }
        }

        public TeamViewModel? SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (!SetProperty(ref _selectedItem, value))
                {
                    return;
                }

                RefreshCommands();
            }
        }

        public EditTeamViewModel? EditTeamViewModel
        {
            get => _editTeamViewModel;
            set
            {
                if (!SetProperty(ref _editTeamViewModel, value))
                {
                    return;
                }

                OnPropertyChanged(nameof(HasEditor));
            }
        }

        public bool HasTeams => Items.Any();

        public bool HasEditor => EditTeamViewModel is not null;

        public object Title => "TODO: Teams";

        public Task LoadDataAsync()
        {
            return _overviewService.LoadDataAsync(this);
        }

        public async Task OnNavigatedToAsync(NavigationMode navigationMode)
        {
            await LoadDataAsync();
            _overviewService.RegisterItemUpdates(this);
        }

        public Task<bool> OnNavigatingFromAsync(NavigationMode navigationMode)
        {
            _overviewService.UnregisterItemUpdates(this);
            return Task.FromResult(true);
        }

        private Task ShowAddTeamAsync()
        {
            return CreateEditTeamViewModel(new TeamParameter());
        }

        private void OnEditTeamViewModelClosed(object? sender, EventArgs e)
        {
            if (EditTeamViewModel is null)
            {
                return;
            }

            EditTeamViewModel.Closed -= OnEditTeamViewModelClosed;
            EditTeamViewModel = null;
        }

        private bool CanEditTeam()
        {
            return SelectedItem is not null;
        }

        private Task ShowEditTeamAsync()
        {
            if (SelectedItem is null)
            {
                return Task.CompletedTask;
            }

            return CreateEditTeamViewModel(new TeamParameter
            {
                TeamId = SelectedItem.Id
            });
        }

        private async Task CreateEditTeamViewModel(TeamParameter parameter)
        {
            if (EditTeamViewModel is not null)
            {
                OnEditTeamViewModelClosed(EditTeamViewModel, EventArgs.Empty);
            }

            EditTeamViewModel = await _navigationService.CreateNavigationTarget<EditTeamViewModel, TeamParameter>(parameter);
            EditTeamViewModel.Closed += OnEditTeamViewModelClosed;
        }

        private bool CanDeleteTeam()
        {
            return SelectedItem is not null;
        }

        private Task DeleteTeamAsync()
        {
            if (SelectedItem is null)
            {
                return Task.CompletedTask;
            }

            return _teamService.DeleteAsync(SelectedItem.Id);
        }

        private void OnItemsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasTeams));
        }

        private void RefreshCommands()
        {
            AddTeamCommand.NotifyCanExecuteChanged();
            EditTeamCommand.NotifyCanExecuteChanged();
            DeleteTeamCommand.NotifyCanExecuteChanged();
        }
    }
}
