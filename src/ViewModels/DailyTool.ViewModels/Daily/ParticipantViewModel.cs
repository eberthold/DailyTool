using CommunityToolkit.Mvvm.ComponentModel;
using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;

namespace DailyTool.ViewModels.Daily
{
    public class ParticipantViewModel : ObservableObject, IParticipant
    {
        private ParticipantMode _participantMode = ParticipantMode.Queued;
        private int _id;
        private string _name = string.Empty;
        private TimeSpan _allocatedTalkStart;
        private TimeSpan _allocatedTalkDuration;
        private double _allocatedTalkProgress;
        private int _index;

        public ParticipantMode ParticipantMode
        {
            get => _participantMode;
            set
            {
                if (!SetProperty(ref _participantMode, value))
                {
                    return;
                }

                OnPropertyChanged(nameof(IsDone));
                OnPropertyChanged(nameof(IsActive));
                OnPropertyChanged(nameof(IsQueued));
            }
        }

        public bool IsDone => ParticipantMode == ParticipantMode.Done;

        public bool IsActive => ParticipantMode == ParticipantMode.Active;

        public bool IsQueued => ParticipantMode == ParticipantMode.Queued;

        public int Id
        {
            get => _id;
            init => SetProperty(ref _id, value);
        }

        public string Name
        {
            get => _name;
            init => SetProperty(ref _name, value);
        }

        public TimeSpan AllocatedTalkStart
        {
            get => _allocatedTalkStart;
            set => SetProperty(ref _allocatedTalkStart, value);
        }

        public TimeSpan AllocatedTalkDuration
        {
            get => _allocatedTalkDuration;
            set => SetProperty(ref _allocatedTalkDuration, value);
        }

        public double AllocatedTalkProgress
        {
            get => _allocatedTalkProgress;
            set => SetProperty(ref _allocatedTalkProgress, value);
        }

        public int Index
        {
            get => _index;
            set => SetProperty(ref _index, value);
        }
    }
}
