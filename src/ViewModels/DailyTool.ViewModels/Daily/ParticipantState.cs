using CommunityToolkit.Mvvm.ComponentModel;
using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;
using System.Collections.ObjectModel;

namespace DailyTool.ViewModels.Daily
{
    public class ParticipantState : ObservableObject, IParticipantState
    {
        private readonly IParticipantService _participantService;
        private readonly IMeetingInfoService _meetingInfoService;

        private bool _isInitialized = false;
        private Dictionary<int, Participant> _participantSource = new Dictionary<int, Participant>();

        public ParticipantState(
            IParticipantService participantService,
            IMeetingInfoService meetingInfoService)
        {
            _participantService = participantService ?? throw new ArgumentNullException(nameof(participantService));
            _meetingInfoService = meetingInfoService ?? throw new ArgumentNullException(nameof(meetingInfoService));
            Participants = new ObservableCollection<ParticipantViewModel>();
        }

        public ObservableCollection<ParticipantViewModel> Participants { get; }

        public async Task LoadDataAsync()
        {
            if (_isInitialized)
            {
                return;
            }

            _isInitialized = true;

            var participants = await _participantService.GetAllAsync();

            foreach (var participant in participants)
            {
                if (_participantSource.ContainsKey(participant.Id))
                {
                    continue;
                }

                _participantSource.Add(participant.Id, participant);

                Participants.Add(ParticipantViewModel.FromParticipant(participant));
            }

            foreach (var participant in _participantSource.Values.ToList())
            {
                if (participants.Any(x => x.Id == participant.Id))
                {
                    continue;
                }

                _participantSource.Remove(participant.Id);
            }

            await ShuffleParticipants();
        }

        public async Task Refresh()
        {
            await _participantService.RefreshParticipantsAsync(_participantSource.Values);

            foreach (var viewModel in Participants.ToList())
            {
                viewModel.MergeFromParticipant(_participantSource[viewModel.Id]);
            }
        }

        public async Task SetNextParticipant()
        {
            await _participantService.SetNextParticipantAsync(_participantSource.Values.OrderBy(x => x.Index).ToList());
            await Refresh();
        }

        public async Task SetPreviousParticipant()
        {
            await _participantService.SetPreviousParticipantAsync(_participantSource.Values.OrderBy(x => x.Index).ToList());
            await Refresh();
        }

        public async Task ShuffleParticipants()
        {
            var meetingInfo = await _meetingInfoService.GetAsync();

            _participantService.ShuffleParticipantsIndex(_participantSource.Values);

            await Refresh();
            foreach (var participant in Participants.ToList())
            {
                var currentIndex = Participants.IndexOf(participant);
                Participants.Move(currentIndex, participant.Index);
            }

            _participantService.CalculateAllocatedTimeSlots(_participantSource.Values.OrderBy(x => x.Index).ToList(), meetingInfo);
            await Refresh();
        }
    }
}
