using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.Infrastructure.Abstractions;
using DailyTool.ViewModels.Abstractions;
using DailyTool.ViewModels.Navigation;
using Scrummy.ViewModels.Shared.Data;
using System.Collections.ObjectModel;

namespace DailyTool.ViewModels.Daily
{
    public class DailyViewModel : ObservableObject, INavigationTarget, ILoadDataAsync, IDisposable
    {
        private readonly IDailyStateProvider _dailyStateProvider;
        private readonly IDailyService _dailyService;
        private readonly IMapper<ParticipantModel, ParticipantViewModel> _viewModelMapper;
        private readonly INavigationService _navigationService;
        private readonly ITimestampProvider _timeStampProvider;
        private readonly IMainThreadInvoker _mainThreadInvoker;

        private DailyState _dailyState = new();

        private Timer? _timer;
        private ObservableCollection<ParticipantViewModel> _participants = new ObservableCollection<ParticipantViewModel>();

        public DailyViewModel(
            IDailyStateProvider dailyStateProvider,
            IDailyService dailyService,
            IMapper<ParticipantModel, ParticipantViewModel> viewModelMapper,
            INavigationService navigationService,
            ITimestampProvider timeStampProvider,
            IMainThreadInvoker mainThreadInvoker)
        {
            _dailyStateProvider = dailyStateProvider;
            _dailyService = dailyService;
            _viewModelMapper = viewModelMapper;
            _navigationService = navigationService;
            _timeStampProvider = timeStampProvider;
            _mainThreadInvoker = mainThreadInvoker;

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

        public Task OnNavigatedToAsync(IReadOnlyDictionary<string, string> parameters, NavigationMode navigationMode)
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
            => _navigationService.NavigateBackAsync();

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
                .Select(_viewModelMapper.Map)
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
                _viewModelMapper.Merge(participant, participantToUpdate);
            }
        }
    }
}