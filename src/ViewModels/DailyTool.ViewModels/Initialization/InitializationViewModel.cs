using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyTool.BusinessLogic.Initialization;
using DailyTool.BusinessLogic.Peoples;

namespace DailyTool.ViewModels.Initialization
{
    public class InitializationViewModel : ObservableObject
    {
        private readonly IInitializationStateController _initializationStateController;
        private InitializationStageState _currentState = new();

        private Person _personToAdd = new();

        public InitializationViewModel(IInitializationStateController initializationStateController)
        {
            _initializationStateController = initializationStateController ?? throw new ArgumentNullException(nameof(initializationStateController));

            AddPersonCommand = new AsyncRelayCommand(AddPersonAsync, CanAddPerson);
            StartMeetingCommand = new AsyncRelayCommand(StartMeetingAsync);
        }

        public IAsyncRelayCommand AddPersonCommand { get; }

        public AsyncRelayCommand StartMeetingCommand { get; }

        public AddPersonViewModel? AddPersonViewModel { get; set; }

        public bool HasAddPersonViewModel => AddPersonViewModel is not null;

        public InitializationStageState CurrentState
        {
            get => _currentState;
            set => SetProperty(ref _currentState, value);
        }

        public async Task LoadDataAsync()
        {
            CurrentState = await _initializationStateController.GetStateAsync().ConfigureAwait(false);
        }

        private bool CanAddPerson()
        {
            return true;
        }

        private Task AddPersonAsync()
        {
            return Task.CompletedTask;
        }

        private Task StartMeetingAsync()
        {
            return _initializationStateController.SaveStateAsync();
        }
    }
}