using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyTool.ViewModels.Abstractions;
using DailyTool.ViewModels.Daily;
using DailyTool.ViewModels.Navigation;

namespace DailyTool.ViewModels.Initialization
{
    public class InitializationViewModel : ObservableObject, INavigationTarget, ILoadDataAsync
    {
        private readonly INavigationService _navigationService;

        public InitializationViewModel(
            IMeetingInfoEditViewModel meetingInfoEditViewModel,
            IPeopleEditViewModel peopleEditViewModel,
            INavigationService navigationService)
        {
            MeetingInfoEditViewModel = meetingInfoEditViewModel ?? throw new ArgumentNullException(nameof(meetingInfoEditViewModel));
            PeopleEditViewModel = peopleEditViewModel ?? throw new ArgumentNullException(nameof(peopleEditViewModel));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            PeopleEditViewModel.PropertyChanged += (_, __) => RefreshCommands();

            StartDailyCommand = new AsyncRelayCommand(StartDailyAsync, CanStartDaily);
        }

        public IAsyncRelayCommand StartDailyCommand { get; }

        public IMeetingInfoEditViewModel MeetingInfoEditViewModel { get; }

        public IPeopleEditViewModel PeopleEditViewModel { get; }

        public async Task LoadDataAsync()
        {
            await Task.WhenAll(new[]
            {
                MeetingInfoEditViewModel.LoadDataAsync(),
                PeopleEditViewModel.LoadDataAsync()
            });

            OnPropertyChanged(string.Empty);
        }

        private Task StartDailyAsync()
        {
            return _navigationService.NavigateAsync<DailyViewModel>();
        }

        public Task OnNavigatedToAsync(NavigationMode navigationMode)
        {
            return Task.CompletedTask;
        }

        public Task<bool> OnNavigatingFromAsync(NavigationMode navigationMode)
        {
            return Task.FromResult(PeopleEditViewModel.IsInViewMode);
        }

        private void RefreshCommands()
        {
            StartDailyCommand.NotifyCanExecuteChanged();
        }

        private bool CanStartDaily()
            => PeopleEditViewModel.IsInViewMode;
    }
}