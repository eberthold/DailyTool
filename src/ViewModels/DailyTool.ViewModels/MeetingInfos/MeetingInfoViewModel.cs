using CommunityToolkit.Mvvm.ComponentModel;

namespace DailyTool.ViewModels.MeetingInfos
{
    public class MeetingInfoViewModel : ObservableObject
    {
        private TimeSpan _startTime;
        private TimeSpan _duration;
        private TimeSpan _endTime;
        private string _sprintBoardUri = string.Empty;

        public TimeSpan StartTime
        {
            get => _startTime;
            set => SetProperty(ref _startTime, value);
        }

        public TimeSpan Duration
        {
            get => _duration;
            set
            {
                if (!SetProperty(ref _duration, value))
                {
                    return;
                }

                _endTime = StartTime + value;
                OnPropertyChanged(nameof(EndTime));
            }
        }

        public TimeSpan EndTime
        {
            get => _endTime;
            set
            {
                if (!SetProperty(ref _endTime, value))
                {
                    return;
                }

                _duration = EndTime - StartTime;
                OnPropertyChanged(nameof(Duration));
            }
        }

        public string SprintBoardUri
        {
            get => _sprintBoardUri;
            set => SetProperty(ref _sprintBoardUri, value);
        }
    }
}
