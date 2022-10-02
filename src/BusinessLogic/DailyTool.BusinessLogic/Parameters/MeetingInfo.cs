using CommunityToolkit.Mvvm.ComponentModel;

namespace DailyTool.BusinessLogic.Parameters
{
    public class MeetingInfo : ObservableObject
    {
        private string _sprintBoardUri = string.Empty;
        private TimeSpan _meetingDuration;
        private TimeSpan _meetingStartTime;

        public TimeSpan MeetingDuration
        {
            get => _meetingDuration;
            set => SetProperty(ref _meetingDuration, value);
        }

        public TimeSpan MeetingStartTime
        {
            get => _meetingStartTime;
            set => SetProperty(ref _meetingStartTime, value);
        }

        public string SprintBoardUri
        {
            get => _sprintBoardUri;
            set => SetProperty(ref _sprintBoardUri, value);
        }
    }
}
