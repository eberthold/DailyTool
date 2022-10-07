using CommunityToolkit.Mvvm.ComponentModel;
using DailyTool.BusinessLogic.Daily.Abstractions;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace DailyTool.ViewModels.Daily
{
    public class ParticipantState : ObservableObject, IParticipantState
    {
        private readonly IParticipantService<ParticipantViewModel> _participantService;
        private readonly IMeetingInfoService _meetingInfoService;

        private bool _isInitialized = false;

        public ParticipantState(
            IParticipantService<ParticipantViewModel> participantService,
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
                Participants.Add(participant);
            }

            await ShuffleParticipants();
        }

        public async Task ShuffleParticipants()
        {
            var meetingInfo = await _meetingInfoService.GetAsync();

            _participantService.ShuffleParticipantsIndex(Participants);

            foreach (var participant in Participants.ToList())
            {
                var currentIndex = Participants.IndexOf(participant);
                Participants.Move(currentIndex, participant.Index);
            }

            _participantService.CalculateAllocatedTimeSlots(Participants, meetingInfo);
        }
    }
}
