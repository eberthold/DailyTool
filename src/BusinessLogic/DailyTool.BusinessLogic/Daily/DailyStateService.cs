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

        public async Task<DailyState> GetDailyStateAsync()
        {
            if (_state is null)
            {
                var meetingInfo = await _meetingInfoRepository.GetAsync().ConfigureAwait(false);
                var participants = await _dataService.GetParticipantsAsync().ConfigureAwait(false);
                ShuffleParticipants(participants);
                participants = participants.OrderBy(x => x.Position).ToList();
                participants.ElementAt(0).IsActiveSpeaker = true;


                _state = new DailyState
                {
                    Participants = new ObservableCollection<Participant>(participants.OrderBy(x => x.Position)),
                    SprintBoardUri = meetingInfo.SprintBoardUri
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
            affectedParticipants.First().IsActiveSpeaker = false;
            affectedParticipants.First().IsDone = true;

            if (affectedParticipants.Count() == 1)
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
    }
}
