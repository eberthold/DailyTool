using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.Infrastructure.Abstractions;

namespace DailyTool.BusinessLogic.Daily
{
    public class ParticipantService : IParticipantService
    {
        private readonly IMeetingParticipantsRepository _repository;
        private readonly ITimestampProvider _timestampProvider;
        private readonly IRandomProvider _randomProvider;

        public ParticipantService(
            IMeetingParticipantsRepository repository,
            ITimestampProvider timestampProvider,
            IRandomProvider randomProvider)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _timestampProvider = timestampProvider ?? throw new ArgumentNullException(nameof(timestampProvider));
            _randomProvider = randomProvider ?? throw new ArgumentNullException(nameof(randomProvider));
        }

        public Task<IReadOnlyCollection<ParticipantModel>> GetParticipantsAsync(int meetingId)
        {
            return _repository.GetParticipantsAsync(meetingId);
        }

        public IReadOnlyCollection<ParticipantModel> ShuffleQueuedParticipants(IReadOnlyCollection<ParticipantModel> participants)
        {
            var rand = _randomProvider.GetRandom();

            var participantsToShuffle = participants.Where(x => x.ParticipantState == ParticipantState.Queued).ToList();
            var unchangedParticipants = participants.Except(participantsToShuffle).ToList();

            int index = 0;
            var result = new List<ParticipantModel>(unchangedParticipants);
            while (participantsToShuffle.Any())
            {
                index = rand.Next(0, participantsToShuffle.Count);
                result.Add(participantsToShuffle[index]);
                participantsToShuffle.RemoveAt(index);
            }

            return result;
        }

        public Task RefreshParticipantsAsync(IReadOnlyCollection<ParticipantModel> participants, DailyMeetingModel meetingInfo)
        {
            var percentagePerParticipant = 100d / participants.Count;
            var elapsedTime = _timestampProvider.CurrentClock - meetingInfo.StartTime;
            var elapsedPercentage = elapsedTime * 100 / meetingInfo.Duration;

            for (var i = 0; i < participants.Count; i++)
            {
                var relativePercentage = elapsedPercentage - (percentagePerParticipant * i);
                var absolutePercentage = relativePercentage * 100 / percentagePerParticipant;
                participants.ElementAt(i).AllocatedProgress = CoerceValidPercentage(absolutePercentage);
            }

            return Task.CompletedTask;
        }

        private double CoerceValidPercentage(double percentage)
        {
            var result = percentage;
            result = Math.Max(0, result);
            result = Math.Min(100, result);

            return result;
        }
    }
}
