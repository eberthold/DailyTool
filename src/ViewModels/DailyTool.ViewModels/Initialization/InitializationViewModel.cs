using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.ViewModels.Abstractions;
using DailyTool.ViewModels.Daily;
using DailyTool.ViewModels.Extensions;
using DailyTool.ViewModels.Navigation;
using System.Collections.ObjectModel;

namespace DailyTool.ViewModels.Initialization
{
    public class InitializationViewModel : ObservableObject, INavigationTarget, ILoadDataAsync
    {
        private readonly IMeetingInfoService _meetingInfoService;
        private readonly IPersonService _personService;
        private readonly INavigationService _navigationService;
        private string _sprintBoardUri = string.Empty;
        private AddPersonViewModel? _addPersonViewModel;
        private PersonViewModel? _selectedPerson;
        private TimeSpan _startTime;
        private TimeSpan _endTime;
        private ObservableCollection<PersonViewModel> _people = new();

        public InitializationViewModel(
            DailyState state,
            IMeetingInfoService meetingInfoService,
            IPersonService personService,
            INavigationService navigationService)
        {
            State = state ?? throw new ArgumentNullException(nameof(state));
            _meetingInfoService = meetingInfoService ?? throw new ArgumentNullException(nameof(meetingInfoService));
            _personService = personService ?? throw new ArgumentNullException(nameof(personService));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            AddPersonCommand = new AsyncRelayCommand(AddPersonAsync, CanAddPerson);
            RemovePersonCommand = new AsyncRelayCommand(RemovePersonAsync, CanRemovePerson);
            StartDailyCommand = new AsyncRelayCommand(StartDailyAsync);
        }

        public IAsyncRelayCommand AddPersonCommand { get; }

        public IAsyncRelayCommand RemovePersonCommand { get; }

        public IAsyncRelayCommand StartDailyCommand { get; }

        public PersonViewModel? SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                if (!SetProperty(ref _selectedPerson, value))
                {
                    return;
                }

                RefreshCommands();
            }
        }

        public AddPersonViewModel? AddPersonViewModel
        {
            get => _addPersonViewModel;
            internal set
            {
                if (!SetProperty(ref _addPersonViewModel, value))
                {
                    return;
                }

                OnPropertyChanged(nameof(IsInPersonAddMode));
                OnPropertyChanged(nameof(IsInViewMode));
                RefreshCommands();
            }
        }

        public bool IsInPersonAddMode => AddPersonViewModel is not null;

        public bool IsInViewMode => AddPersonViewModel is null;

        public TimeSpan EndTime
        {
            get => _endTime;
            set
            {
                if (!SetProperty(ref _endTime, value))
                {
                    return;
                }

                var info = State.MeetingInfo with
                {
                    MeetingDuration = value.Subtract(StartTime),
                };

                _meetingInfoService.UpdateAsync(info, State);
            }
        }

        public TimeSpan StartTime
        {
            get => _startTime;
            set
            {
                if (!SetProperty(ref _startTime, value))
                {
                    return;
                }

                var info = State.MeetingInfo with
                {
                    MeetingStartTime = value
                };

                _meetingInfoService.UpdateAsync(info, State);

                EndTime = StartTime.Add(State.MeetingInfo.MeetingDuration);
            }
        }

        public string SprintBoardUri
        {
            get => _sprintBoardUri;
            set
            {
                if (!SetProperty(ref _sprintBoardUri, value))
                {
                    return;
                }

                var info = State.MeetingInfo with
                {
                    SprintBoardUri = value
                };

                _meetingInfoService.UpdateAsync(info, State);
            }
        }

        public DailyState State { get; }

        public ObservableCollection<PersonViewModel> People
        {
            get => _people;
            set => SetProperty(ref _people, value);
        }

        public async Task LoadDataAsync()
        {
            await _meetingInfoService.LoadAsync(State);

            await LoadPeopleAsync();

            _startTime = State.MeetingInfo.MeetingStartTime;
            _endTime = StartTime.Add(State.MeetingInfo.MeetingDuration);
            _sprintBoardUri = State.MeetingInfo.SprintBoardUri;

            OnPropertyChanged(string.Empty);
        }

        private async Task LoadPeopleAsync()
        {
            var people = await _personService.GetAllAsync();
            var mappedPeople = people.Select(x =>
            {
                var viewModel = x.ToViewModel();
                viewModel.PropertyChanged += OnPersonChanged;
                return viewModel;
            });

            People = new ObservableCollection<PersonViewModel>(mappedPeople);
        }

        private bool CanAddPerson()
        {
            return IsInViewMode;
        }

        private async Task AddPersonAsync()
        {
            AddPersonViewModel = await _navigationService.CreateNavigationTarget<AddPersonViewModel>();
            AddPersonViewModel.AddCloseCallback(() => OnAddPersonClosed());
        }

        private Task StartDailyAsync()
        {
            return _navigationService.NavigateAsync<DailyViewModel>();
        }

        public Task OnNavigatedToAsync(NavigationMode navigationMode)
        {
            return Task.CompletedTask;
        }

        public Task<bool> OnNavigatingFromAsync(NavigationMode navigationMode)
        {
            return Task.FromResult(IsInViewMode);
        }

        private void RefreshCommands()
        {
            AddPersonCommand.NotifyCanExecuteChanged();
            RemovePersonCommand.NotifyCanExecuteChanged();
            StartDailyCommand.NotifyCanExecuteChanged();
        }

        private Task OnAddPersonClosed()
        {
            if (AddPersonViewModel?.AddedPerson is null)
            {
                return Task.CompletedTask;
            }

            People.Add(AddPersonViewModel.AddedPerson);
            AddPersonViewModel = null;
            return Task.CompletedTask;
        }

        private bool CanRemovePerson()
        {
            return SelectedPerson is not null;
        }

        private async Task RemovePersonAsync()
        {
            if (SelectedPerson is null)
            {
                return;
            }

            SelectedPerson.PropertyChanged -= OnPersonChanged;
            await _personService.DeletePersonAsync(SelectedPerson.Id);
            People.Remove(SelectedPerson);
        }

        private void OnPersonChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var person = sender as PersonViewModel;
            if (person is null)
            {
                return;
            }

            _personService.UpdatePersonAsync(person.ToBusinessObject());
        }
    }
}