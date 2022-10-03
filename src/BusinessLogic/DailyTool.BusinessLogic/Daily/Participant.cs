using CommunityToolkit.Mvvm.ComponentModel;

namespace DailyTool.BusinessLogic.Daily
{
    public class Participant : ObservableObject
    {
        private bool _isActiveSpeaker;
        private bool _isDone;
        private bool _isQueued = true;
        private double _allocatedTalkProgress;

        /// <summary>
        /// Gets a value indicating whether the participant has finished his daily talk already.
        /// </summary>
        public bool IsDone
        {
            get => _isDone;
            internal set => SetProperty(ref _isDone, value);
        }

        /// <summary>
        /// Gets a value indicating whether it's the participants turn.
        /// </summary>
        public bool IsActiveSpeaker
        {
            get => _isActiveSpeaker;
            internal set => SetProperty(ref _isActiveSpeaker, value);
        }

        /// <summary>
        /// Gets a value indicating whether the participant is an upcoming speaker.
        /// </summary>
        public bool IsQueued
        {
            get => _isQueued;
            internal set => SetProperty(ref _isQueued, value);
        }

        public string Name { get; init; } = string.Empty;

        public TimeSpan AllocatedTalkStart { get; internal set; }

        public TimeSpan AllocatedTalkDuration { get; internal set; }

        public double AllocatedTalkProgress
        {
            get => _allocatedTalkProgress;
            internal set => SetProperty(ref _allocatedTalkProgress, value);
        }

        public int Position { get; internal set; }
    }
}
