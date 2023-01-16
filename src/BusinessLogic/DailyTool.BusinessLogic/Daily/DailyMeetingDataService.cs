using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.Infrastructure.Abstractions;
using Scrummy.Core.BusinessLogic.Meeting;

namespace DailyTool.BusinessLogic.Daily
{
    public class DailyMeetingDataService : IDailyMeetingDataService
    {
        private readonly IDailyMeetingRepository _repository;
        private readonly ITimestampProvider _timeStampProvider;

        public DailyMeetingDataService(
            IDailyMeetingRepository repository,
            ITimestampProvider timeStampProvider)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _timeStampProvider = timeStampProvider ?? throw new ArgumentNullException(nameof(timeStampProvider));
        }

        public Task<DailyMeetingModel> GetByIdAsync(int meetingId)
        {
            return _repository.GetByIdAsync(meetingId);
        }

        public Task UpdateAsync(DailyMeetingModel meetingInfo)
        {
            return _repository.UpdateAsync(meetingInfo);
        }

        public double CalculateMeetingPercentage(DailyMeetingModel meetingInfo)
        {
            var elapsed = _timeStampProvider.CurrentClock - meetingInfo.StartTime;

            if (meetingInfo.Duration.TotalMilliseconds <= 0)
            {
                return 100;
            }

            return elapsed.TotalMilliseconds * 100d / meetingInfo.Duration.TotalMilliseconds;
        }

        public Task<DailyMeetingModel> GetByTeamAsync(int teamId)
        {
            return _repository.GetByTeamAsync(teamId);
        }
    }
}
