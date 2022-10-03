using DailyTool.BusinessLogic.Parameters;
using System.Collections.ObjectModel;

namespace DailyTool.BusinessLogic.Daily
{
    public class DailyStateService : IDailyStateService
    {
        private readonly IDailyDataService _dataService;
        private readonly IMeetingInfoRepository _meetingInfoRepository;

        private DailyState? _state;

        public DailyStateService(
            IDailyDataService dataService,
            IMeetingInfoRepository meetingInfoRepository)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            _meetingInfoRepository = meetingInfoRepository ?? throw new ArgumentNullException(nameof(meetingInfoRepository));
        }

        public async Task<DailyState> GetDailyStateAsync(bool force = false)
        {
            if (_state is null || force)
            {
                var meetingInfo = await _meetingInfoRepository.GetAsync().ConfigureAwait(false);
                var participants = await _dataService.GetParticipantsAsync().ConfigureAwait(false);
                ShuffleParticipants(participants);
                participants = participants.OrderBy(x => x.Position).ToList();
                participants.ElementAt(0).IsActiveSpeaker = true;

                await InitializeParticipants(participants, meetingInfo).ConfigureAwait(false);

                _state = new DailyState
                {
                    Participants = new ObservableCollection<Participant>(participants),
                    SprintBoardUri = meetingInfo.SprintBoardUri,
                    MeetingInfo = meetingInfo
                };
            }

            return _state;
        }

        public async Task RefreshStateAsync()
        {
            if (_state is null)
            {
                return;
            }

            var refreshTasks = _state.Participants.Select(x => _dataService.RefreshParticipantAsync(x));
            await Task.WhenAll(refreshTasks);
        }

        public Task SetNextParticipantAsync()
        {
            if (_state is null)
            {
                return Task.CompletedTask;
            }

            var affectedParticipants = _state.Participants.SkipWhile(x => !x.IsActiveSpeaker).Take(2).ToList();
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

        private void ShuffleParticipants(IReadOnlyCollection<Participant> participants)
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
        }

        private async Task InitializeParticipants(IReadOnlyCollection<Participant> participants, MeetingInfo meetingInfo)
        {
            var averageTalkTime = await _dataService.CalculateAverageTalkDuration(meetingInfo, participants).ConfigureAwait(false);
            for (var i = 0; i < participants.Count; i++)
            {
                var participant = participants.ElementAt(i);
                participant.AllocatedTalkDuration = averageTalkTime;
                participant.AllocatedTalkStart = meetingInfo.MeetingStartTime + (averageTalkTime * i);
            }
        }
    }
}
