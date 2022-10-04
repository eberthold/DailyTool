using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.BusinessLogic.System;

namespace DailyTool.BusinessLogic.Daily
{
    public class DailyService : IDailyService
    {
        private readonly ITimeStampProvider _timeStampProvider;
        private readonly IParticipantRepository _participantRepository;

        public DailyService(
            ITimeStampProvider timeStampProvider,
            IParticipantRepository participantRepository)
        {
            _timeStampProvider = timeStampProvider ?? throw new ArgumentNullException(nameof(timeStampProvider));
            _participantRepository = participantRepository ?? throw new ArgumentNullException(nameof(participantRepository));
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

        public Task RefreshParticipantsAsync(DailyState state)
        {
            foreach (var participant in state.Participants)
            {
                var elapsed = _timeStampProvider.CurrentClock - participant.AllocatedTalkStart;
                var percentage = elapsed.TotalMilliseconds * 100d / participant.AllocatedTalkDuration.TotalMilliseconds;
                percentage = Math.Min(percentage, 100);
                percentage = Math.Max(percentage, 0);
                participant.AllocatedTalkProgress = percentage;
            }

            return Task.CompletedTask;
        }

        public Task SetNextParticipantAsync(DailyState state)
        {
            var affectedParticipants = state.Participants.SkipWhile(x => !x.IsActiveSpeaker).Take(2).ToList();
            if (affectedParticipants.Count == 0)
            {
                return Task.CompletedTask;
            }

            affectedParticipants.First().IsActiveSpeaker = false;
            affectedParticipants.First().IsDone = true;

            if (affectedParticipants.Count == 1)
            {
                return Task.CompletedTask;
            }

            affectedParticipants.Last().IsActiveSpeaker = true;
            affectedParticipants.Last().IsQueued = false;

            return Task.CompletedTask;
        }
    }
}
