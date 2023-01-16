using FluentAssertions;
using Scrummy.Core.ViewModels.Navigation;
using System.Globalization;

namespace Scrummy.Core.ViewModels.Tests.Navigation
{
    [TestClass]
    public class NavigationParameterSerializerTests
    {
        [TestMethod]
        public void Serialize_HappyPath_ReturnsExpectedResult()
        {
            // Arrange
            var source = new TestObject
            {
                Decimal = 3.5d,
                Flag = true,
                Number = 7,
                Text = "itzl",
            };

            var separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            var expected = $"Decimal=3{separator}5&Flag=True&Number=7&Text=itzl";

            // Act
            var result = NavigationParameterSerializer.Serialize(source);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
        
        [TestMethod]
        public void Deserialize_HappyPath_ReturnsExpectedResult()
        {
            // Arrange
            var separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            var source = $"Decimal=3{separator}5&Flag=True&Number=7&Text=itzl";

            var expected = new TestObject
            {
                Decimal = 3.5d,
                Flag = true,
                Number = 7,
                Text = "itzl",
            };

            // Act
            var result = NavigationParameterSerializer.Deserialize<TestObject>(source);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        public class TestObject
        {
            public double Decimal { get; set; }

            public bool Flag { get; set; }

            public int Number { get; set; }

            public string Text { get; set; } = string.Empty;
        }
    }
}
