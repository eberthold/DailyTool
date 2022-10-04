using DailyTool.BusinessLogic.Daily.Abstractions;

namespace DailyTool.BusinessLogic.Daily
{
    public class MeetingInfoService : IMeetingInfoService
    {
        private readonly IMeetingInfoRepository _repository;

        public MeetingInfoService(IMeetingInfoRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task LoadAsync(DailyState state)
        {
            state.MeetingInfo = await _repository.GetAsync();
        }

        public Task UpdateAsync(MeetingInfo meetingInfo, DailyState state)
        {
            state.MeetingInfo = meetingInfo;
            return _repository.SaveAsync(meetingInfo);
        }
    }
}
