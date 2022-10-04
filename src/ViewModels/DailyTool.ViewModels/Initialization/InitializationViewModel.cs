using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.ViewModels.Abstractions;
using DailyTool.ViewModels.Daily;
using DailyTool.ViewModels.Navigation;

namespace DailyTool.ViewModels.Initialization
{
    public class InitializationViewModel : ObservableObject, INavigationTarget, ILoadDataAsync
    {
        private readonly IMeetingInfoService _meetingInfoService;
        private readonly IPersonService _personService;
        private readonly INavigationService _navigationService;

        private AddPersonViewModel? _addPersonViewModel;
        private Person? _selectedPerson;
        private TimeSpan _startTime;
        private TimeSpan _endTime;

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

        public Person? SelectedPerson
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

        public DailyState State { get; }

        public async Task LoadDataAsync()
        {
            await _meetingInfoService.LoadAsync(State);
            await _personService.LoadAllAsync(State);

            StartTime = State.MeetingInfo.MeetingStartTime;
            EndTime = StartTime.Add(State.MeetingInfo.MeetingDuration);
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
            AddPersonViewModel = null;
            return Task.CompletedTask;
        }

        private bool CanRemovePerson()
        {
            return SelectedPerson is not null;
        }

        private Task RemovePersonAsync()
        {
            if (SelectedPerson is null)
            {
                return Task.CompletedTask;
            }

            return _personService.RemovePersonAsync(SelectedPerson, State);
        }
    }
}