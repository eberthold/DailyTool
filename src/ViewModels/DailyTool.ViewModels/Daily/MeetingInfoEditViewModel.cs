using CommunityToolkit.Mvvm.ComponentModel;
using DailyTool.ViewModels.Navigation;

namespace DailyTool.ViewModels.Daily
{
    public class MeetingInfoEditViewModel : ObservableObject, INavigationTarget
    {
        public MeetingInfoEditViewModel(
            IMeetingInfoState state)
        {
            State = state ?? throw new ArgumentNullException(nameof(state));
        }

        public IMeetingInfoState State { get; }

        public Task LoadDataAsync()
        {
            return State.LoadDataAsync();
        }

        public Task OnNavigatedToAsync(NavigationMode navigationMode)
        {
            return Task.CompletedTask;
        }

        public Task<bool> OnNavigatingFromAsync(NavigationMode navigationMode)
        {
            return Task.FromResult(true);
        }
    }
}
