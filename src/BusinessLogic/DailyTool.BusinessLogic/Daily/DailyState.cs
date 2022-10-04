using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace DailyTool.BusinessLogic.Daily
{
    public class DailyState : ObservableObject
    {
        private MeetingInfo _meetingInfo = new MeetingInfo();
        private ObservableCollection<Person> _editablePeople = null!;
        private ReadOnlyObservableCollection<Person> _people = null!;
        private ObservableCollection<Participant> _editableParticipants = null!;
        private ReadOnlyObservableCollection<Participant> _participants = null!;

        public DailyState()
        {
            EditablePeople = new ObservableCollection<Person>();
            EditableParticipants = new ObservableCollection<Participant>();
        }

        internal ObservableCollection<Person> EditablePeople
        {
            get => _editablePeople;
            set
            {
                _editablePeople = value;
                People = new ReadOnlyObservableCollection<Person>(value);
            }
        }

        internal ObservableCollection<Participant> EditableParticipants
        {
            get => _editableParticipants;
            set
            {
                _editableParticipants = value;
                Participants = new ReadOnlyObservableCollection<Participant>(value);
            }
        }

        /// <summary>
        /// Gets all persons which are known to the application.
        /// </summary>
        public ReadOnlyObservableCollection<Person> People
        {
            get => _people;
            private set => SetProperty(ref _people, value);
        }

        /// <summary>
        /// Gets only persons which are participating in the meeting.
        /// </summary>
        public ReadOnlyObservableCollection<Participant> Participants
        {
            get => _participants;
            private set => SetProperty(ref _participants, value);
        }

        /// <summary>
        /// Gets basic meeting information.
        /// </summary>
        public MeetingInfo MeetingInfo
        {
            get => _meetingInfo;
            internal set => SetProperty(ref _meetingInfo, value);
        }
    }
}
