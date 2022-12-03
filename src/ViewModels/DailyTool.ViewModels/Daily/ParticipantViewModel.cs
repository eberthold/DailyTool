using CommunityToolkit.Mvvm.ComponentModel;

namespace DailyTool.ViewModels.Daily
{
    public class ParticipantViewModel : ObservableObject
    {
        private int _id;
        private string _name = string.Empty;
        private TimeSpan _allocatedTalkStart;
        private TimeSpan _allocatedTalkDuration;
        private double _allocatedTalkProgress;
        private int _index;
        private bool _isDone;
        private bool _isActive;
        private bool _isQueued;

        public bool IsDone
        {
            get => _isDone;
            set => SetProperty(ref _isDone, value);
        }

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        public bool IsQueued
        {
            get => _isQueued;
            set => SetProperty(ref _isQueued, value);
        }

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

        public double AllocatedProgress
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