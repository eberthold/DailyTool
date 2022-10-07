using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.BusinessLogic.System;

namespace DailyTool.BusinessLogic.Daily
{
    public class MeetingInfoService : IMeetingInfoService
    {
        private readonly IMeetingInfoRepository _repository;
        private readonly ITimeStampProvider _timeStampProvider;

        public MeetingInfoService(
            IMeetingInfoRepository repository,
            ITimeStampProvider timeStampProvider)
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
            var elapsed = _timeStampProvider.CurrentClock - meetingInfo.MeetingStartTime;

            if (meetingInfo.MeetingDuration.TotalMilliseconds <= 0)
            {
                return 100;
            }

            return elapsed.TotalMilliseconds * 100d / meetingInfo.MeetingDuration.TotalMilliseconds;
        }
    }
}
