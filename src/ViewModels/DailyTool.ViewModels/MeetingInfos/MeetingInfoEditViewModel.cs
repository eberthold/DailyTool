using CommunityToolkit.Mvvm.ComponentModel;
using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.Infrastructure.Abstractions;
using DailyTool.ViewModels.Abstractions;
using Scrummy.Core.BusinessLogic.Meeting;
using Scrummy.Core.ViewModels.Navigation;
using Scrummy.Core.ViewModels.Parameters;
using Scrummy.ViewModels.Shared.Data;

namespace DailyTool.ViewModels.MeetingInfos
{
    public class MeetingInfoEditViewModel : ObservableObject, ILoadDataAsync, ISaveDataAsync, INavigationTarget<MeetingParameter>
    {
        private readonly IDailyMeetingDataService _meetingInfoService;
        private readonly IMapper _mapper;
        private readonly ITaskQueue _taskQueue;

        private MeetingInfoViewModel _meetingInfo = new();
        private int _meetingId = 0;
        private int _teamId = 0;
        private KnownMeetingType _meetingType = KnownMeetingType.Daily;

        public MeetingInfoEditViewModel(
            IDailyMeetingDataService meetingInfoService,
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
            var meetingInfo = await _meetingInfoService.GetByTeamAsync(_teamId);
            MeetingInfo = _mapper.Map<MeetingInfoViewModel>(meetingInfo);
        }

        public Task SaveDataAsync()
        {
            var meetingInfo = _mapper.Map<DailyMeetingModel>(MeetingInfo);
            return _meetingInfoService.UpdateAsync(meetingInfo);
        }

        private void QueueSave()
        {
            _taskQueue.Enqueue(() => SaveDataAsync());

            _taskQueue.ProcessQueueAsync(new());
        }

        public Task OnNavigatedToAsync(MeetingParameter parameter, NavigationMode navigationMode)
        {
            _teamId = parameter.TeamId;
            _meetingType = parameter.MeetingType;

            return LoadDataAsync();
        }

        public Task<bool> OnNavigatingFromAsync(NavigationMode navigationMode)
            => Task.FromResult(true);
    }
}
