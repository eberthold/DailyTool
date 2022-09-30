using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyTool.BusinessLogic.Initialization;
using DailyTool.BusinessLogic.Peoples;

namespace DailyTool.ViewModels.Initialization
{
    public class AddPersonViewModel : ObservableObject
    {
        private readonly IInitializationStateController _initializationStateController;

        private string _name = string.Empty;

        public AddPersonViewModel(IInitializationStateController initializationStateController)
        {
            _initializationStateController = initializationStateController ?? throw new ArgumentNullException(nameof(initializationStateController));
            AddPersonCommand = new AsyncRelayCommand(AddPersonAsync, CanAddPerson);
        }

        public IAsyncRelayCommand AddPersonCommand { get; }

        public string Name
        {
            get => _name;
            set
            {
                if (!SetProperty(ref _name, value))
                {
                    return;
                }

                RefreshCommands();
            }
        }

        private bool CanAddPerson()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }

        private async Task AddPersonAsync()
        {
            var person = new Person
            {
                Name = Name
            };

            await _initializationStateController.AddPersonAsync(person);

            Name = string.Empty;
        }

        private void RefreshCommands()
        {
            AddPersonCommand.NotifyCanExecuteChanged();
        }
    }
}
