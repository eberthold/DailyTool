using DailyTool.BusinessLogic.Daily;
using DailyTool.BusinessLogic.Daily.Abstractions;
using DailyTool.Infrastructure.Abstractions;
using FluentAssertions;
using NSubstitute;

namespace DailyTool.BusinessLogic.Tests
{
    [TestClass]
    public class ParticipantServiceTests
    {
        private readonly IParticipantRepository _participantRepository = Substitute.For<IParticipantRepository>();
        private readonly ITimestampProvider _timestampProvider = Substitute.For<ITimestampProvider>();
        private readonly IRandomProvider _randomProvider = Substitute.For<IRandomProvider>();

        [TestMethod]
        public async Task RefreshParticipantsAsync_HappyPath_CaluclatesExpectedPercentages()
        {
            // Arrange
            var sut = CreateSut();
            var meetingInfo = new MeetingInfo
            {
                StartTime = new TimeSpan(9, 0, 0),
                Duration = new TimeSpan(0, 10, 0)
            };

            // make percentages of 25 for easier testing
            var participants = new List<Participant>
            {
                new(), new(), new(), new()
            };

            _timestampProvider.CurrentClock.Returns(new TimeSpan(9, 3, 45));

            // Act
            await sut.RefreshParticipantsAsync(participants, meetingInfo);

            // Assert
            participants[0].AllocatedProgress.Should().Be(100);
            participants[1].AllocatedProgress.Should().Be(50);
            participants[2].AllocatedProgress.Should().Be(0);
            participants[3].AllocatedProgress.Should().Be(0);
        }

        [TestMethod]
        public void ShuffleQueuedParticipants_WithDoneAndActiveParticipants_ReturnsShuffeledList()
        {
            // Arrange
            var sut = CreateSut();

            var participants = new List<Participant>
            {
                new Participant { Id = 1, ParticipantState = ParticipantState.Done },
                new Participant { Id = 2, ParticipantState = ParticipantState.Done },
                new Participant { Id = 3, ParticipantState = ParticipantState.Active },
                new Participant { Id = 4 },
                new Participant { Id = 5 },
                new Participant { Id = 6 },
                new Participant { Id = 7 },
            };

            // fix seed for predictable results
            var random = new Random(21);
            _randomProvider.GetRandom().Returns(random);

            // Act
            var result = sut.ShuffleQueuedParticipants(participants);

            // Assert
            result.Select(x => x.Id).Should().BeEquivalentTo(
                new[]
                {
                    1,
                    2,
                    3,
                    6,
                    7,
                    4,
                    5,
                },
                options => options.WithStrictOrdering());
        }

        private ParticipantService CreateSut()
        {
            return new ParticipantService(
                _participantRepository,
                _timestampProvider,
                _randomProvider);
        }
    }
}
