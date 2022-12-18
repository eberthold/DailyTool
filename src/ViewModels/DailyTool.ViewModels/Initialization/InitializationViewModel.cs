using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyTool.ViewModels.Abstractions;
using DailyTool.ViewModels.Daily;
using DailyTool.ViewModels.MeetingInfos;
using DailyTool.ViewModels.Navigation;
using DailyTool.ViewModels.People;
using Scrummy.ViewModels.Shared.Data;

namespace DailyTool.ViewModels.Initialization
{
    public class InitializationViewModel : ObservableObject, INavigationTarget, ILoadDataAsync, ITitle
    {
        private readonly INavigationService _navigationService;
        private MeetingInfoEditViewModel? _meetingInfoEditViewModel;
        private PeopleOverviewViewModel? _peopleEditViewModel;

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

        public string Title => "TODO: Daily Vorbereitung";

        public async Task LoadDataAsync()
        {
            MeetingInfoEditViewModel = await _navigationService.CreateNavigationTarget<MeetingInfoEditViewModel>();
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

        public Task OnNavigatedToAsync(IReadOnlyDictionary<string, string> parameters, NavigationMode navigationMode)
        {
            return Task.CompletedTask;
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