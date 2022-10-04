using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.ViewModels.Abstractions;
using DailyTool.ViewModels.Navigation;

namespace DailyTool.ViewModels.Initialization
{
    public class AddPersonViewModel : ObservableObject, INavigationTarget, INotifyClose
    {
        private readonly List<Func<Task>> _closeCallbacks = new List<Func<Task>>();
        private readonly IPersonService _personService;
        private string _name = string.Empty;

        public AddPersonViewModel(
            DailyState state,
            IPersonService personService)
        {
            State = state;
            _personService = personService ?? throw new ArgumentNullException(nameof(personService));

            AddPersonCommand = new AsyncRelayCommand(AddPersonAsync, CanAddPerson);
            CancelCommand = new AsyncRelayCommand(CancelAsync);
        }

        public IAsyncRelayCommand AddPersonCommand { get; }

        public IAsyncRelayCommand CancelCommand { get; }

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

        public DailyState State { get; }

        private bool CanAddPerson()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }

        private async Task AddPersonAsync()
        {
            var person = new Person
            {
                Name = Name,
                IsParticipating = true
            };

            await _personService.AddPersonAsync(person, State);
            await OnCloseAsync();
        }

        private void RefreshCommands()
        {
            AddPersonCommand.NotifyCanExecuteChanged();
        }

        private Task CancelAsync()
        {
            return OnCloseAsync();
        }

        public void AddCloseCallback(Func<Task> callback)
        {
            _closeCallbacks.Add(callback);
        }

        private async Task OnCloseAsync()
        {
            var tasks = _closeCallbacks.Select(x => x()).ToList();
            _closeCallbacks.Clear();
            await Task.WhenAll(tasks);
        }

        public Task OnNavigatedToAsync(NavigationMode navigationMode)
        {
            return Task.CompletedTask;
        }

        public Task<bool> OnNavigatingFromAsync(NavigationMode navigationMode)
        {
            return Task.FromResult(true);
        }
    }
}
