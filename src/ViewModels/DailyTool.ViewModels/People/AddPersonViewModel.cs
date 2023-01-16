using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.ViewModels.Abstractions;
using Scrummy.Core.ViewModels.Navigation;

namespace DailyTool.ViewModels.People
{
    public class AddPersonViewModel : ObservableObject, INavigationTarget, INotifyClose
    {
        private readonly IPersonService _personService;
        private string _name = string.Empty;

        public AddPersonViewModel(IPersonService personService)
        {
            AddPersonCommand = new AsyncRelayCommand(AddPersonAsync, CanAddPerson);
            CancelCommand = new AsyncRelayCommand(CancelAsync);

            _personService = personService ?? throw new ArgumentNullException(nameof(personService));
        }

        public event EventHandler? Closed;

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

        private bool CanAddPerson()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }

        private async Task AddPersonAsync()
        {
            var person = new PersonModel
            {
                Name = Name,
            };

            await _personService.CreatePersonAsync(person);
            Closed?.Invoke(this, EventArgs.Empty);
        }

        private void RefreshCommands()
        {
            AddPersonCommand.NotifyCanExecuteChanged();
        }

        private Task CancelAsync()
        {
            Closed?.Invoke(this, EventArgs.Empty);
            return Task.CompletedTask;
        }

        public Task OnNavigatedToAsync(IReadOnlyDictionary<string, string> parameters, NavigationMode navigationMode)
        {
            return Task.CompletedTask;
        }

        public Task<bool> OnNavigatingFromAsync(NavigationMode navigationMode)
        {
            return Task.FromResult(true);
        }
    }
}
