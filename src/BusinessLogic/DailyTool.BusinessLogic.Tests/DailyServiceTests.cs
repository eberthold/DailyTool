using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;
using FluentAssertions;
using NSubstitute;

namespace DailyTool.BusinessLogic.Tests
{
    [TestClass]
    public class DailyServiceTests
    {
        private readonly IParticipantService _participantService = Substitute.For<IParticipantService>();
        private readonly IMeetingInfoService _meetinInfoService = Substitute.For<IMeetingInfoService>();

        [TestMethod]
        public async Task ShuffleParticipants_HappyPath_CallsParticipantService()
        {
            // Arrange
            var sut = CreateSut();
            var originalParticipants = new List<Participant>();
            var shuffledParticipants = new List<Participant>();
            var state = new DailyState
            {
                OrderedParticipants = originalParticipants
            };

            _participantService.ShuffleQueuedParticipants(originalParticipants).Returns(shuffledParticipants);

            // Act
            await sut.ShuffleParticipantsAsync(state);

            // Assert
            _participantService.Received(1).ShuffleQueuedParticipants(originalParticipants);
            state.OrderedParticipants.Should().BeSameAs(shuffledParticipants);
        }

        [TestMethod]
        public async Task InitializeParticipants_Shuffled_LoadsAndShufflesParticipants()
        {
            // Arrange
            var sut = CreateSut();
            var state = new DailyState();
            var originalParticipants = new List<Participant>();
            var shuffledParticipants = new List<Participant>();
            _participantService.GetAllAsync().Returns(originalParticipants);
            _participantService.ShuffleQueuedParticipants(originalParticipants).Returns(shuffledParticipants);

            var settings = new ParticipantInitializationSettings
            {
                Shuffle = true
            };

            // Act
            await sut.InitializeParticipantsAsync(state, settings);

            // Assert
            state.OrderedParticipants.Should().BeSameAs(shuffledParticipants);
        }

        [TestMethod]
        public async Task InitializeMeetingInfo_HappyPath_MeetingInfoSetOnState()
        {
            // Arrange
            var sut = CreateSut();
            var state = new DailyState();
            var meetingInfo = new MeetingInfo();

            _meetinInfoService.GetAsync().Returns(meetingInfo);

            // Act
            await sut.InitializeMeetingInfoAsync(state);

            // Assert
            state.MeetingInfo.Should().BeSameAs(meetingInfo);
        }

        [TestMethod]
        public async Task RefreshState_HappyPath_TriggersParticipantService()
        {
            // Arrange
            var sut = CreateSut();
            var state = new DailyState
            {
                OrderedParticipants = new List<Participant>(),
                MeetingInfo = new MeetingInfo()
            };

            // Act
            await sut.RefreshStateAsync(state);

            // Assert
            await _participantService.Received(1).RefreshParticipantsAsync(state.OrderedParticipants, state.MeetingInfo);
        }

        [TestMethod]
        [DataRow(1, ParticipantState.Active, ParticipantState.Queued, ParticipantState.Queued)]
        [DataRow(2, ParticipantState.Done, ParticipantState.Active, ParticipantState.Queued)]
        [DataRow(3, ParticipantState.Done, ParticipantState.Done, ParticipantState.Active)]
        [DataRow(4, ParticipantState.Done, ParticipantState.Done, ParticipantState.Done)]
        [DataRow(5, ParticipantState.Done, ParticipantState.Done, ParticipantState.Done)]
        public async Task SetNextParticipantAsync_TimesCalled_ResultAsExpected(int executionCount, params ParticipantState[] result)
        {
            // Arrange
            var sut = CreateSut();

            var participants = new List<Participant>
            {
                new(), new(), new()
            };

            // Act
            for (var i = 0; i < executionCount; i++)
            {
                await sut.SetNextParticipantAsync(participants);
            }

            // Assert
            participants
                .Select(x => x.ParticipantState)
                .Should()
                .BeEquivalentTo(result);
        }

        [TestMethod]
        [DataRow(1, ParticipantState.Done, ParticipantState.Done, ParticipantState.Active)]
        [DataRow(2, ParticipantState.Done, ParticipantState.Active, ParticipantState.Queued)]
        [DataRow(3, ParticipantState.Active, ParticipantState.Queued, ParticipantState.Queued)]
        [DataRow(4, ParticipantState.Queued, ParticipantState.Queued, ParticipantState.Queued)]
        [DataRow(5, ParticipantState.Queued, ParticipantState.Queued, ParticipantState.Queued)]
        public async Task SetPreviousParticipantAsync_TimesCalled_ResultAsExpected(int executionCount, params ParticipantState[] result)
        {
            // Arrange
            var sut = CreateSut();

            var participants = new List<Participant>
            {
                new() { ParticipantState = ParticipantState.Done },
                new() { ParticipantState = ParticipantState.Done },
                new() { ParticipantState = ParticipantState.Done },
            };

            // Act
            for (var i = 0; i < executionCount; i++)
            {
                await sut.SetPreviousParticipantAsync(participants);
            }

            // Assert
            participants
                .Select(x => x.ParticipantState)
                .Should()
                .BeEquivalentTo(result);
        }

        private DailyService CreateSut()
        {
            return new DailyService(
                _participantService,
                _meetinInfoService);
        }
    }
}
