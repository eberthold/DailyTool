using DailyTool.DataAccess.Meetings;
using DailyTool.DataAccess.People;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DailyTool.DataAccess.Tests.Meetings
{
    [TestClass]
    public class MeetingParticipantsRepositoryTests
    {
        private IDbContextFactory<DatabaseContext> _contextFactory = new InMemoryDbContextFactory();

        [TestMethod]
        public async Task AddParticipantAsync_HappyPath_AddsParticipant()
        {
            // Arrange
            var sut = CreateSut();

            await AddParticipantTestCore(sut);
        }

        private async Task<int> AddParticipantTestCore(MeetingParticipantsRepository sut)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var person = new PersonEntity
                {
                    Id = 5,
                    Name = "PersonName"
                };
                context.People.Add(person);

                var team = new DataAccess.Teams.TeamEntity
                {
                    Id = 3,
                    Name = "Team1",
                    Members = new List<PersonEntity> { person }
                };
                context.Teams.Add(team);

                var meeting = new MeetingEntity
                {
                    Id = 8,
                    Team = team
                };
                context.Meetings.Add(meeting);

                await context.SaveChangesAsync();
            }

            // Act
            await sut.AddParticipantAsync(8, 5);

            // Assert
            using (var context = _contextFactory.CreateDbContext())
            {
                var result = context.MeetingParticipants.Single();
                result.PersonId.Should().Be(5);
                result.DailyMeetingId.Should().Be(8);
                result.Id.Should().Be(1);

                return result.Id;
            }
        }

        [TestMethod]
        public async Task RemoveParticipant_HappyPath_Removed()
        {
            // Arrange
            var sut = CreateSut();

            var id = await AddParticipantTestCore(sut);

            // Act
            await sut.RemoveParticipantAsync(id);

            // Assert
            using (var context = _contextFactory.CreateDbContext())
            {
                context.MeetingParticipants.Count().Should().Be(0);
            }
        }

        [TestMethod]
        public async Task GetParticipantsAsync_HappyPath_ReturnsParticipants()
        {
            // Arrange
            var sut = CreateSut();

            var id = await AddParticipantTestCore(sut);

            // Act
            var result = await sut.GetParticipantsAsync(8);

            // Assert
            result.Count().Should().Be(1);
            var resultItem = result.Single();
            resultItem.Id.Should().Be(id);
            resultItem.Name.Should().Be("PersonName");
        }

        private MeetingParticipantsRepository CreateSut()
        {
            return new MeetingParticipantsRepository(
                _contextFactory);
        }
    }
}
