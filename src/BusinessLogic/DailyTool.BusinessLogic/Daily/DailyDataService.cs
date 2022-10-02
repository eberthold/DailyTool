using DailyTool.BusinessLogic.Parameters;
using DailyTool.BusinessLogic.System;

namespace DailyTool.BusinessLogic.Daily
{
    public class DailyDataService : IDailyDataService
    {
        private readonly ITimeStampProvider _timeStampProvider;
        private readonly IParticipantRepository _participantRepository;

        public DailyDataService(
            ITimeStampProvider timeStampProvider,
            IParticipantRepository participantRepository)
        {
            _timeStampProvider = timeStampProvider ?? throw new ArgumentNullException(nameof(timeStampProvider));
            _participantRepository = participantRepository ?? throw new ArgumentNullException(nameof(participantRepository));
        }

        public Task<TimeSpan> CalculateAverageTalkDuration(MeetingInfo meetingInfo, IReadOnlyCollection<Participant> participants)
        {
            var averageTalkTime = meetingInfo.MeetingDuration / participants.Count;

            // trim milliseconds, they don't matter anyways
            averageTalkTime = averageTalkTime.Subtract(TimeSpan.FromMilliseconds(averageTalkTime.Milliseconds));

            return Task.FromResult(averageTalkTime);
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

        public Task<IReadOnlyCollection<Participant>> GetParticipantsAsync()
        {
            return _participantRepository.GetAllAsync();
        }

        public Task RefreshParticipantAsync(Participant participant)
        {
            if (participant.IsDone || participant.IsActiveSpeaker)
            {
                return Task.CompletedTask;
            }

            participant.ActualTalkDuration = _timeStampProvider.CurrentClock - participant.TalkingStartedAt;
            return Task.CompletedTask;
        }
    }
}
