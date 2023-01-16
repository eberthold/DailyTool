using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyTool.ViewModels.Abstractions;
using DailyTool.ViewModels.Daily;
using DailyTool.ViewModels.MeetingInfos;
using DailyTool.ViewModels.People;
using Scrummy.Core.BusinessLogic.Meeting;
using Scrummy.Core.ViewModels.Navigation;
using Scrummy.Core.ViewModels.Parameters;
using Scrummy.ViewModels.Shared.Data;

namespace DailyTool.ViewModels.Initialization
{
    public class InitializationViewModel : ObservableObject, INavigationTarget<TeamParameter>, ILoadDataAsync, ITitle
    {
        private readonly INavigationService _navigationService;
        private MeetingInfoEditViewModel? _meetingInfoEditViewModel;
        private PeopleOverviewViewModel? _peopleEditViewModel;
        private int _teamId;

        public InitializationViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            StartDailyCommand = new AsyncRelayCommand(StartDailyAsync, CanStartDaily);
        }

        public IAsyncRelayCommand StartDailyCommand { get; }

        public MeetingInfoEditViewModel? MeetingInfoEditViewModel
        {
            get => _meetingInfoEditViewModel;
            private set => SetProperty(ref _meetingInfoEditViewModel, value);
        }

        public PeopleOverviewViewModel? PeopleEditViewModel
        {
            get => _peopleEditViewModel;
            private set => SetProperty(ref _peopleEditViewModel, value);
        }

        public object Title => "TODO: Daily Vorbereitung";

        public async Task LoadDataAsync()
        {
            MeetingInfoEditViewModel = await _navigationService.CreateNavigationTarget<MeetingInfoEditViewModel, MeetingParameter>(new MeetingParameter
            {
                TeamId = _teamId,
                MeetingType = KnownMeetingType.Daily
            });

            PeopleEditViewModel = await _navigationService.CreateNavigationTarget<PeopleOverviewViewModel>();

            PeopleEditViewModel.PropertyChanged += (_, __) => RefreshCommands();

            await Task.WhenAll(new[]
            {
                MeetingInfoEditViewModel.LoadDataAsync(),
                PeopleEditViewModel.LoadDataAsync()
            });

            RefreshCommands();
        }

        private Task StartDailyAsync()
        {
            return _navigationService.NavigateAsync<DailyViewModel>();
        }

        public Task OnNavigatedToAsync(TeamParameter parameter, NavigationMode navigationMode)
        {
            _teamId = parameter.TeamId;

            return LoadDataAsync();
        }

        public Task<bool> OnNavigatingFromAsync(NavigationMode navigationMode)
        {
            return Task.FromResult(PeopleEditViewModel?.IsInViewMode ?? true);
        }

        private void RefreshCommands()
        {
            StartDailyCommand.NotifyCanExecuteChanged();
        }

        private bool CanStartDaily()
            => PeopleEditViewModel?.IsInViewMode ?? false;
    }
}