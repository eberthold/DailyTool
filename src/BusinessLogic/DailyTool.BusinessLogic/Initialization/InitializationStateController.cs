using CommunityToolkit.Mvvm.Messaging;
using DailyTool.BusinessLogic.People;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DailyTool.BusinessLogic.Initialization
{
    public class InitializationStateController : IInitializationStateController, IRecipient<InitializationStateChangedMessage>
    {
        private readonly IInitializationService _initializationService;
        private readonly IMessenger _messenger;
        private InitializationStageState? _stageState;

        public InitializationStateController(
            IInitializationService initializationService,
            IMessenger messenger)
        {
            _initializationService = initializationService ?? throw new ArgumentNullException(nameof(initializationService));
            _messenger = messenger ?? throw new ArgumentNullException(nameof(messenger));

            _messenger.Register(this);
        }

        public Task AddPersonAsync(Person person)
        {
            if (_stageState is null)
            {
                return Task.CompletedTask;
            }

            person.PropertyChanged += OnStateObjectChanged;
            _stageState.People.Add(person);
            return SaveStateAsync();
        }

        public async Task<InitializationStageState> GetStateAsync()
        {
            if (_stageState is null)
            {
                var people = await _initializationService.GetPeopleAsync();
                var meetingInfo = await _initializationService.GetMeetingInfoAsync();

                meetingInfo.PropertyChanged += OnStateObjectChanged;
                foreach (var person in people)
                {
                    person.PropertyChanged += OnStateObjectChanged;
                }

                _stageState = new InitializationStageState
                {
                    People = new ObservableCollection<Person>(people.OrderBy(x => x.Name)),
                    MeetingInfo = meetingInfo
                };
            }

            return _stageState;
        }

        public Task RemovePersonAsync(Person person)
        {
            if (_stageState is null)
            {
                return Task.CompletedTask;
            }

            person.PropertyChanged -= OnStateObjectChanged;
            _stageState.People.Remove(person);
            return SaveStateAsync();
        }

        public async Task SaveStateAsync()
        {
            if (_stageState is null)
            {
                return;
            }

            await _initializationService.SavePeopleAsync(_stageState.People);
            await _initializationService.SaveMeetingInfoAsync(_stageState.MeetingInfo);
        }

        private void OnStateObjectChanged(object? sender, PropertyChangedEventArgs e)
        {
            _messenger.Send(new InitializationStateChangedMessage());
        }

        async void IRecipient<InitializationStateChangedMessage>.Receive(InitializationStateChangedMessage message)
        {
            await SaveStateAsync();
        }
    }
}
