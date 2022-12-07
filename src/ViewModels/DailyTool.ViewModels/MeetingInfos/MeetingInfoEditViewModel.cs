using CommunityToolkit.Mvvm.ComponentModel;
using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.Infrastructure.Abstractions;
using DailyTool.ViewModels.Abstractions;
using DailyTool.ViewModels.Navigation;
using Scrummy.ViewModels.Shared.Data;

namespace DailyTool.ViewModels.MeetingInfos
{
    public class MeetingInfoEditViewModel : ObservableObject, ILoadDataAsync, ISaveDataAsync, INavigationTarget
    {
        private readonly IMeetingInfoService _meetingInfoService;
        private readonly IMapper _mapper;
        private readonly ITaskQueue _taskQueue;

        private MeetingInfoViewModel _meetingInfo = new();

        public MeetingInfoEditViewModel(
            IMeetingInfoService meetingInfoService,
            IMapper mapper,
            ITaskQueue taskQueue)
        {
            _meetingInfoService = meetingInfoService ?? throw new ArgumentNullException(nameof(meetingInfoService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _taskQueue = taskQueue ?? throw new ArgumentNullException(nameof(taskQueue));

            MeetingInfo = new();
        }

        public MeetingInfoViewModel MeetingInfo
        {
            get => _meetingInfo;
            set
            {
                _meetingInfo.PropertyChanged -= OnMeetingInfoChanged;

                if (!SetProperty(ref _meetingInfo, value))
                {
                    return;
                }

                value.PropertyChanged += OnMeetingInfoChanged;
            }
        }

        private void OnMeetingInfoChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            QueueSave();
        }

        public async Task LoadDataAsync()
        {
            var meetingInfo = await _meetingInfoService.GetAsync();
            MeetingInfo = _mapper.Map<MeetingInfoViewModel>(meetingInfo);
        }

        public Task SaveDataAsync()
        {
            var meetingInfo = _mapper.Map<MeetingInfo>(MeetingInfo);
            return _meetingInfoService.UpdateAsync(meetingInfo);
        }

        private void QueueSave()
        {
            _taskQueue.Enqueue(() => SaveDataAsync());

            _taskQueue.ProcessQueueAsync(new());
        }

        public Task OnNavigatedToAsync(NavigationMode navigationMode)
            => Task.CompletedTask;

        public Task<bool> OnNavigatingFromAsync(NavigationMode navigationMode)
            => Task.FromResult(true);
    }
}
