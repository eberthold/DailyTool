using DailyTool.DataAccess.Generic;
using DailyTool.DataAccess.Teams;
using FluentAssertions;
using Scrummy.Core.BusinessLogic.Teams;

namespace DailyTool.DataAccess.Tests.Teams
{
    [TestClass]
    public class TeamRepositoryTests
    {
        private readonly TeamDataAccessMapper _mapper = new();
        private readonly InMemoryDbContextFactory _contextFactory = new();

        [TestMethod]
        public async Task CreateAsync_HappyPath_Created()
        {
            // Arrange
            var sut = CreateSut();

            var model = new TeamModel
            {
                Id = 1,
                Name = "teamsy"
            };

            // Act
            var result = await sut.CreateAsync(model);

            // Assert
            result.Should().BeGreaterThan(0);

            using (var assertContext = _contextFactory.CreateDbContext())
            {
                var loadedModel = assertContext.Teams.Single();

                loadedModel.Should().BeEquivalentTo(model);
            }
        }

        [TestMethod]
        public async Task DeleteAsync_HappyPath_Deleted()
        {
            // Arrange
            var sut = CreateSut();

            var model = new TeamModel
            {
                Id = 1,
                Name = "teamsy"
            };

            var idToDelete = await sut.CreateAsync(model);

            // Act
            await sut.DeleteAsync(idToDelete);

            // Assert
            using (var assertContext = _contextFactory.CreateDbContext())
            {
                var loadedModel = assertContext.Teams.Any().Should().BeFalse();
            }
        }

        [TestMethod]
        public async Task GetAllAsync_HappyPath_ReturnsResult()
        {
            // Arrange
            var sut = CreateSut();
            await EnsureTeamsEntry();

            // Act
            var result = await sut.GetAllAsync();

            // Assert
            result.Should().NotBeEmpty();
        }

        [TestMethod]
        public async Task GetByIdAsync_HappyPath_ReturnsResult()
        {
            // Arrange
            var sut = CreateSut();
            await EnsureTeamsEntry();

            // Act
            var result = await sut.GetByIdAsync(999);

            // Assert
            result.Id.Should().Be(999);
            result.Name.Should().Be("teamsy");
        }

        [TestMethod]
        public async Task UpdateAsync_HappyPath_ReturnsResult()
        {
            // Arrange
            var sut = CreateSut();
            await EnsureTeamsEntry();

            var model = new TeamModel
            {
                Id = 999,
                Name = "teamsyNeu"
            };

            // Act
            await sut.UpdateAsync(model);

            // Assert
            using (var assertContext = _contextFactory.CreateDbContext())
            {
                var result = await sut.GetByIdAsync(999);
                result.Id.Should().Be(999);
                result.Name.Should().Be("teamsyNeu");
            }
        }

        private Task EnsureTeamsEntry()
        {
            var model = new TeamEntity
            {
                Id = 999,
                Name = "teamsy"
            };

            using (var context = _contextFactory.CreateDbContext())
            {
                context.Teams.Add(model);
                return context.SaveChangesAsync();
            }
        }

        private GenericRepository<TeamModel, TeamEntity> CreateSut()
        {
            return new GenericRepository<TeamModel, TeamEntity>(
                _contextFactory,
                _mapper,
                _mapper);
        }
    }
}
