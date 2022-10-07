using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.BusinessLogic.System;

namespace DailyTool.BusinessLogic.Daily
{
    public class ParticipantService<T> : IParticipantService<T>
        where T : IParticipant
    {
        private readonly IPersonRepository _repository;
        private readonly IParticipantFactory<T> _participantFactory;
        private readonly ITimeStampProvider _timeStampProvider;

        public ParticipantService(
            IPersonRepository repository,
            IParticipantFactory<T> participantFactory,
            ITimeStampProvider timeStampProvider)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _participantFactory = participantFactory ?? throw new ArgumentNullException(nameof(participantFactory));
            _timeStampProvider = timeStampProvider ?? throw new ArgumentNullException(nameof(timeStampProvider));
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            var persons = await _repository.GetAllAsync();
            return persons.Select(_participantFactory.Create).ToList();
        }

        public void ShuffleParticipantsIndex(IReadOnlyCollection<T> participants)
        {
            var rand = new Random();
            var positions = new HashSet<int>();
            while (positions.Count < participants.Count)
            {
                var position = rand.Next(0, participants.Count);
                if (positions.Contains(position))
                {
                    continue;
                }

                positions.Add(position);
            }

            for (var i = 0; i < positions.Count; i++)
            {
                participants.ElementAt(i).Index = positions.ElementAt(i);
            }
        }

        public void CalculateAllocatedTimeSlots(IReadOnlyCollection<T> participants, MeetingInfo meetingInfo)
        {
            var averageTalkTime = CalculateAverageTalkDuration(meetingInfo, participants);
            for (var i = 0; i < participants.Count; i++)
            {
                var participant = participants.ElementAt(i);
                participant.AllocatedTalkDuration = averageTalkTime;
                participant.AllocatedTalkStart = (averageTalkTime * i) + meetingInfo.MeetingStartTime;
            }
        }

        private TimeSpan CalculateAverageTalkDuration(MeetingInfo meetingInfo, IReadOnlyCollection<T> participants)
        {
            var averageTalkTime = meetingInfo.MeetingDuration / participants.Count;

            return averageTalkTime;
        }

        public Task RefreshParticipantsAsync(IReadOnlyCollection<T> participants)
        {
            foreach (var participant in participants)
            {
                participant.AllocatedTalkProgress = CalculateAllocatedProgressForParticipant(participant);
            }

            return Task.CompletedTask;
        }

        private double CalculateAllocatedProgressForParticipant(T participant)
        {
            var elapsed = _timeStampProvider.CurrentClock - participant.AllocatedTalkStart;
            var percentage = elapsed.TotalMilliseconds * 100d / participant.AllocatedTalkDuration.TotalMilliseconds;
            percentage = Math.Min(percentage, 100);
            percentage = Math.Max(percentage, 0);
            return percentage;
        }

        public Task SetPreviousParticipantAsync(IReadOnlyCollection<T> participants)
        {
            var deactivedParticipant = participants.FirstOrDefault(x => x.ParticipantMode == ParticipantMode.Active);
            if (deactivedParticipant is not null)
            {
                deactivedParticipant.ParticipantMode = ParticipantMode.Queued;
            }

            var reactivatedParticipant = participants.LastOrDefault(x => x.ParticipantMode == ParticipantMode.Done);
            if (reactivatedParticipant is not null)
            {
                reactivatedParticipant.ParticipantMode = ParticipantMode.Active;
            }

            return Task.CompletedTask;
        }

        public Task SetNextParticipantAsync(IReadOnlyCollection<T> participants)
        {
            var doneParticipant = participants.FirstOrDefault(x => x.ParticipantMode == ParticipantMode.Active);
            if (doneParticipant is not null)
            {
                doneParticipant.ParticipantMode = ParticipantMode.Done;
            }

            var activedParticipant = participants.FirstOrDefault(x => x.ParticipantMode == ParticipantMode.Queued);
            if (activedParticipant is not null)
            {
                activedParticipant.ParticipantMode = ParticipantMode.Active;
            }

            return Task.CompletedTask;
        }
    }
}
