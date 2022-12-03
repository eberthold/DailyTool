using DailyTool.BusinessLogic.Daily.Abstractions;

namespace DailyTool.BusinessLogic.Daily
{
    public class DailyService : IDailyService
    {
        private readonly IParticipantService _participantService;
        private readonly IMeetingInfoService _meetingInfoService;

        public DailyService(
            IParticipantService participantService,
            IMeetingInfoService meetingInfoService)
        {
            _participantService = participantService ?? throw new ArgumentNullException(nameof(participantService));
            _meetingInfoService = meetingInfoService ?? throw new ArgumentNullException(nameof(meetingInfoService));
        }

        public Task ShuffleParticipantsAsync(DailyState state)
        {
            state.OrderedParticipants = _participantService.ShuffleQueuedParticipants(state.OrderedParticipants);
            return Task.CompletedTask;
        }

        public async Task InitializeParticipantsAsync(DailyState state, ParticipantInitializationSettings settings)
        {
            state.OrderedParticipants = await _participantService.GetAllAsync().ConfigureAwait(false);

            if (settings.Shuffle)
            {
                await ShuffleParticipantsAsync(state);
            }
        }

        public async Task InitializeMeetingInfoAsync(DailyState state)
        {
            state.MeetingInfo = await _meetingInfoService.GetAsync();
        }

        public async Task RefreshStateAsync(DailyState state)
        {
            await _participantService.RefreshParticipantsAsync(state.OrderedParticipants, state.MeetingInfo);
        }

        public Task SetPreviousParticipantAsync(IReadOnlyCollection<Participant> participants)
        {
            var requeuedParticipant = participants.FirstOrDefault(x => x.ParticipantState == ParticipantState.Active);
            if (requeuedParticipant is not null)
            {
                requeuedParticipant.ParticipantState = ParticipantState.Queued;
            }

            var reactivatedParticipant = participants.LastOrDefault(x => x.ParticipantState == ParticipantState.Done);
            if (reactivatedParticipant is not null)
            {
                reactivatedParticipant.ParticipantState = ParticipantState.Active;
            }

            return Task.CompletedTask;
        }

        public Task SetNextParticipantAsync(IReadOnlyCollection<Participant> participants)
        {
            var doneParticipant = participants.FirstOrDefault(x => x.ParticipantState == ParticipantState.Active);
            if (doneParticipant is not null)
            {
                doneParticipant.ParticipantState = ParticipantState.Done;
            }

            var activedParticipant = participants.FirstOrDefault(x => x.ParticipantState == ParticipantState.Queued);
            if (activedParticipant is not null)
            {
                activedParticipant.ParticipantState = ParticipantState.Active;
            }

            return Task.CompletedTask;
        }
    }
}
