using DailyTool.Infrastructure.Abstractions;
using FluentAssertions;
using NSubstitute;

namespace DailyTool.Infrastructure.Tests;

[TestClass]
public class GenericMapperTests
{
    private IServiceProvider _serviceProvicer = Substitute.For<IServiceProvider>();

    [TestMethod]
    public void Map_HappyPath_ReturnsExpectedResult()
    {
        // Arrange
        var sut = CreateSut();

        var mapper = Substitute.For<IMapper<Type1, Type2>>();
        _serviceProvicer.GetService(typeof(IMapper<Type1, Type2>)).Returns(mapper);

        var object1 = new Type1();
        var object2 = new Type2();

        mapper.Map(object1).Returns(object2);

        // Act
        var result = sut.Map<Type2>(object1);

        // Assert
        result.Should().BeSameAs(object2);
    }

    private GenericMapper CreateSut()
    {
        return new GenericMapper(
            _serviceProvicer);
    }

    public class Type1 { }

    public class Type2 { }
}