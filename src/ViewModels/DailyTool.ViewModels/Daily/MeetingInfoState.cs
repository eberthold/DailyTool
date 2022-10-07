using CommunityToolkit.Mvvm.ComponentModel;
using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;

namespace DailyTool.ViewModels.Daily
{
    public class MeetingInfoState : ObservableObject, IMeetingInfoState
    {
        private readonly IMeetingInfoService _meetingInfoService;
        private readonly Queue<Func<Task>> _updateQueue = new();
        private readonly SemaphoreSlim _semaphore = new(1);

        private TimeSpan _startTime;
        private TimeSpan _duration;
        private string _sprintBoardUri = string.Empty;
        private bool _isInitialized = false;

        public MeetingInfoState(IMeetingInfoService meetingInfoService)
        {
            _meetingInfoService = meetingInfoService;
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

                OnPropertyChanged(nameof(EndTime));
                QueueUpdate();
            }
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

                OnPropertyChanged(nameof(EndTime));
                QueueUpdate();
            }
        }

        public TimeSpan EndTime
        {
            get => StartTime + Duration;
            set
            {
                Duration = value - StartTime;
                OnPropertyChanged();
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

                QueueUpdate();
            }
        }

        private void QueueUpdate()
        {
            var meetingInfo = new MeetingInfo
            {
                MeetingDuration = Duration,
                MeetingStartTime = StartTime,
                SprintBoardUri = SprintBoardUri
            };

            _updateQueue.Enqueue(() => _meetingInfoService.UpdateAsync(meetingInfo));
            _ = ProcessQueue();
        }

        private async Task ProcessQueue()
        {
            await _semaphore.WaitAsync().ConfigureAwait(false);

            try
            {
                if (_updateQueue.Count == 0)
                {
                    return;
                }

                Func<Task>? task;
                while (_updateQueue.Count > 0)
                {
                    task = _updateQueue.Dequeue();
                    await task().ConfigureAwait(false);
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task LoadDataAsync()
        {
            if (_isInitialized)
            {
                return;
            }

            _isInitialized = true;

            var meetingInfo = await _meetingInfoService.GetAsync();
            _startTime = meetingInfo.MeetingStartTime;
            _duration = meetingInfo.MeetingDuration;
            _sprintBoardUri = meetingInfo.SprintBoardUri;

            OnPropertyChanged(string.Empty);
        }
    }
}
