using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.Infrastructure.Abstractions;

namespace DailyTool.BusinessLogic.Daily
{
    public class MeetingInfoService : IMeetingInfoService
    {
        private readonly IMeetingInfoRepository _repository;
        private readonly ITimestampProvider _timeStampProvider;

        public MeetingInfoService(
            IMeetingInfoRepository repository,
            ITimestampProvider timeStampProvider)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _timeStampProvider = timeStampProvider ?? throw new ArgumentNullException(nameof(timeStampProvider));
        }

        public Task<MeetingInfo> GetAsync()
        {
            return _repository.GetAsync();
        }

        public Task UpdateAsync(MeetingInfo meetingInfo)
        {
            return _repository.SaveAsync(meetingInfo);
        }

        public double CalculateMeetingPercentage(MeetingInfo meetingInfo)
        {
            var elapsed = _timeStampProvider.CurrentClock - meetingInfo.StartTime;

            if (meetingInfo.Duration.TotalMilliseconds <= 0)
            {
                return 100;
            }

            return elapsed.TotalMilliseconds * 100d / meetingInfo.Duration.TotalMilliseconds;
        }
    }
}
