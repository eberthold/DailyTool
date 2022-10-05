using CommunityToolkit.Mvvm.ComponentModel;
using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.ViewModels.Navigation;

namespace DailyTool.ViewModels.Daily
{
    public class MeetingInfoEditViewModel : ObservableObject, IMeetingInfoEditViewModel
    {
        private readonly IMeetingInfoService _meetingInfoService;

        private string _sprintBoardUri = string.Empty;
        private TimeSpan _startTime;
        private TimeSpan _endTime;

        public MeetingInfoEditViewModel(
            DailyState state,
            IMeetingInfoService meetingInfoService)
        {
            State = state ?? throw new ArgumentNullException(nameof(state));
            _meetingInfoService = meetingInfoService ?? throw new ArgumentNullException(nameof(meetingInfoService));
        }

        public DailyState State { get; }

        public TimeSpan EndTime
        {
            get => _endTime;
            set
            {
                if (!SetProperty(ref _endTime, value))
                {
                    return;
                }

                var info = State.MeetingInfo with
                {
                    MeetingDuration = value.Subtract(StartTime),
                };

                _meetingInfoService.UpdateAsync(info, State);
            }
        }

        public TimeSpan StartTime
        {
            get => _startTime;
            set
            {
                if (!SetProperty(ref _startTime, value))
                {
                    return;
                }

                var info = State.MeetingInfo with
                {
                    MeetingStartTime = value
                };

                _meetingInfoService.UpdateAsync(info, State);

                EndTime = StartTime.Add(State.MeetingInfo.MeetingDuration);
            }
        }

        public string SprintBoardUri
        {
            get => _sprintBoardUri;
            set
            {
                if (!SetProperty(ref _sprintBoardUri, value))
                {
                    return;
                }

                var info = State.MeetingInfo with
                {
                    SprintBoardUri = value
                };

                _meetingInfoService.UpdateAsync(info, State);
            }
        }

        public async Task LoadDataAsync()
        {
            await _meetingInfoService.LoadAsync(State);

            // update with backing fields to avoid service updates
            _startTime = State.MeetingInfo.MeetingStartTime;
            _endTime = StartTime.Add(State.MeetingInfo.MeetingDuration);
            _sprintBoardUri = State.MeetingInfo.SprintBoardUri;

            OnPropertyChanged(string.Empty);
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
