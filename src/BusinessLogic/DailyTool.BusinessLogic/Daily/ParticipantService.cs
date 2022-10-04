using DailyTool.BusinessLogic.Daily.Abstractions;
using System.Collections.ObjectModel;

namespace DailyTool.BusinessLogic.Daily
{
    public class ParticipantService : IParticipantService
    {
        private readonly IParticipantRepository _repository;

        public ParticipantService(IParticipantRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task LoadParticipantsForMeetingAsync(MeetingInfo meetingInfo, DailyState state)
        {
            var participants = await _repository.GetAllAsync();

            participants = ShuffleParticipants(participants);

            // TODO: move to set next speaker
            participants.ElementAt(0).IsActiveSpeaker = true;

            InitializeParticipants(participants, meetingInfo);

            state.EditableParticipants = new ObservableCollection<Participant>(participants);
        }

        private IReadOnlyCollection<Participant> ShuffleParticipants(IReadOnlyCollection<Participant> participants)
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
                participants.ElementAt(i).Position = positions.ElementAt(i);
            }

            return participants = participants.OrderBy(x => x.Position).ToList();
        }

        private void InitializeParticipants(IReadOnlyCollection<Participant> participants, MeetingInfo meetingInfo)
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
    }
}
