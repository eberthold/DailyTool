using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.Infrastructure.Abstractions;
using DailyTool.ViewModels.Abstractions;
using DailyTool.ViewModels.Navigation;
using System.Collections.ObjectModel;

namespace DailyTool.ViewModels.Daily
{
    public class DailyViewModel : ObservableObject, INavigationTarget, ILoadDataAsync, IDisposable
    {
        private readonly IDailyStateProvider _dailyStateProvider;
        private readonly IDailyService _dailyService;
        private readonly IMapper _mapper;
        private readonly IMerger<Participant, ParticipantViewModel> _participantMerger;
        private readonly INavigationService _navigationService;
        private readonly ITimestampProvider _timeStampProvider;
        private readonly IMainThreadInvoker _mainThreadInvoker;

        private DailyState _dailyState = new();

        private Timer? _timer;
        private ObservableCollection<ParticipantViewModel> _participants = new ObservableCollection<ParticipantViewModel>();

        public DailyViewModel(
            IDailyStateProvider dailyStateProvider,
            IDailyService dailyService,
            IMapper mapper,
            IMerger<Participant, ParticipantViewModel> participantMerger,
            INavigationService navigationService,
            ITimestampProvider timeStampProvider,
            IMainThreadInvoker mainThreadInvoker)
        {
            _dailyStateProvider = dailyStateProvider ?? throw new ArgumentNullException(nameof(dailyStateProvider));
            _dailyService = dailyService ?? throw new ArgumentNullException(nameof(dailyService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _participantMerger = participantMerger ?? throw new ArgumentNullException(nameof(participantMerger));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _timeStampProvider = timeStampProvider ?? throw new ArgumentNullException(nameof(timeStampProvider));
            _mainThreadInvoker = mainThreadInvoker ?? throw new ArgumentNullException(nameof(mainThreadInvoker));

            NavigateBackCommand = new AsyncRelayCommand(NavigateBackAsync);
            PreviousSpeakerCommand = new AsyncRelayCommand(SetPreviousParticipantAsync);
            NextSpeakerCommand = new AsyncRelayCommand(SetNextParticipantAsync);
            ShuffleParticipantsCommand = new AsyncRelayCommand(ShuffleParticipantsAsync);
        }

        public AsyncRelayCommand NavigateBackCommand { get; }

        public AsyncRelayCommand PreviousSpeakerCommand { get; }

        public AsyncRelayCommand NextSpeakerCommand { get; }

        public AsyncRelayCommand ShuffleParticipantsCommand { get; }

        public string Time => _timeStampProvider.CurrentClock.ToString(@"hh\:mm\:ss");

        public ObservableCollection<ParticipantViewModel> Participants
        {
            get => _participants;
            set => SetProperty(ref _participants, value);
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public async Task LoadDataAsync()
        {
            _dailyState = await _dailyStateProvider.GetAsync().ConfigureAwait(true);

            await ShuffleParticipantsAsync();

            _timer = new Timer(OnTimerElapsed);
            _timer.Change(0, 100);
        }

        public Task OnNavigatedToAsync(NavigationMode navigationMode)
         => Task.CompletedTask;

        public Task<bool> OnNavigatingFromAsync(NavigationMode navigationMode)
         => Task.FromResult(true);

        private void OnTimerElapsed(object? state)
        {
            _mainThreadInvoker.InvokeAsync(() =>
            {
                OnPropertyChanged(nameof(Time));
                _dailyService.RefreshStateAsync(_dailyState);
                return RefreshParticipants();
            });
        }

        private Task NavigateBackAsync()
            => _navigationService.GoBackAsync();

        private async Task SetPreviousParticipantAsync()
        {
            await _dailyService.SetPreviousParticipantAsync(_dailyState.OrderedParticipants);
            await RefreshParticipants();
        }

        private async Task SetNextParticipantAsync()
        {
            await _dailyService.SetNextParticipantAsync(_dailyState.OrderedParticipants);
            await RefreshParticipants();
        }

        private async Task ShuffleParticipantsAsync()
        {
            await _dailyService.ShuffleParticipantsAsync(_dailyState);
            var mappedParticipants = _dailyState
                .OrderedParticipants
                .Select(_mapper.Map<ParticipantViewModel>)
                .ToList();

            Participants = new ObservableCollection<ParticipantViewModel>(mappedParticipants);

            await RefreshParticipants();
        }

        private async Task RefreshParticipants()
        {
            await _dailyService.RefreshStateAsync(_dailyState);

            foreach (var participant in _dailyState.OrderedParticipants)
            {
                var participantToUpdate = Participants.Single(x => x.Id == participant.Id);
                _participantMerger.Merge(participantToUpdate, participant);
            }
        }
    }
}