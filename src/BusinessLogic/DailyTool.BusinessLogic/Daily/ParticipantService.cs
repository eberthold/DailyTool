using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.BusinessLogic.System;

namespace DailyTool.BusinessLogic.Daily
{
    public class ParticipantService : IParticipantService
    {
        private readonly IPersonRepository _repository;
        private readonly ITimeStampProvider _timeStampProvider;

        public ParticipantService(
            IPersonRepository repository,
            ITimeStampProvider timeStampProvider)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _timeStampProvider = timeStampProvider ?? throw new ArgumentNullException(nameof(timeStampProvider));
        }

        public async Task<IReadOnlyCollection<Participant>> GetAllAsync()
        {
            var persons = await _repository.GetAllAsync();

            return persons
                .Select(person =>
                    new Participant
                    {
                        Id = person.Id,
                        Name = person.Name
                    })
                .ToList();
        }

        public void ShuffleParticipantsIndex(IReadOnlyCollection<Participant> participants)
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

        public void CalculateAllocatedTimeSlots(IReadOnlyCollection<Participant> participants, MeetingInfo meetingInfo)
        {
            var averageTalkTime = CalculateAverageTalkDuration(meetingInfo, participants);
            for (var i = 0; i < participants.Count; i++)
            {
                var participant = participants.ElementAt(i);
                participant.AllocatedTalkDuration = averageTalkTime;
                participant.AllocatedTalkStart = (averageTalkTime * i) + meetingInfo.MeetingStartTime;
            }
        }

        private TimeSpan CalculateAverageTalkDuration(MeetingInfo meetingInfo, IReadOnlyCollection<Participant> participants)
        {
            var averageTalkTime = meetingInfo.MeetingDuration / participants.Count;

            return averageTalkTime;
        }

        public Task RefreshParticipantsAsync(IReadOnlyCollection<Participant> participants)
        {
            foreach (var participant in participants)
            {
                participant.AllocatedTalkProgress = CalculateAllocatedProgressForParticipant(participant);
            }

            return Task.CompletedTask;
        }

        private double CalculateAllocatedProgressForParticipant(Participant participant)
        {
            var elapsed = _timeStampProvider.CurrentClock - participant.AllocatedTalkStart;
            var percentage = elapsed.TotalMilliseconds * 100d / participant.AllocatedTalkDuration.TotalMilliseconds;
            percentage = Math.Min(percentage, 100);
            percentage = Math.Max(percentage, 0);
            return percentage;
        }

        public Task SetPreviousParticipantAsync(IReadOnlyCollection<Participant> participants)
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

        public Task SetNextParticipantAsync(IReadOnlyCollection<Participant> participants)
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
