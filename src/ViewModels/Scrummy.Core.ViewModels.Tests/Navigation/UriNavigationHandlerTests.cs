using FluentAssertions;
using Scrummy.Core.ViewModels.Navigation;

namespace Scrummy.Core.ViewModels.Tests.Navigation
{
    [TestClass]
    public class UriNavigationHandlerTests
    {
        [TestMethod]
        public void TryMatchParameters_Match_ReturnsTrueAndSetsParameter()
        {
            // Arrange
            var sut = CreateSut();
            var template = "/super/{id}/sub";
            var uri = "/super/7/sub";

            // Act
            var result = sut.TryMatchParameters<TestParameter>(template, uri, out var resultParameter);

            // Assert
            result.Should().Be(true);
            resultParameter.Should().NotBeNull();
            resultParameter!.Id.Should().Be(7);
        }

        private UriNavigationHandler CreateSut()
        {
            return new UriNavigationHandler();
        }

        public class TestParameter
        {
            public int Id { get; set; }
        }
    }
}
