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
                Id = 8,
                Name = "teamsy"
            };

            // Act
            var result = await sut.CreateAsync(model);

            // Assert
            result.Should().BeGreaterThan(0);

            using (var assertContext = _contextFactory.Create())
            {
                var loadedModel = assertContext.Teams.Single();
                loadedModel.Should().BeEquivalentTo(model);
            }
        }

        private TeamRepository CreateSut()
        {
            return new TeamRepository(
                _contextFactory,
                _mapper,
                _mapper);
        }
    }
}
