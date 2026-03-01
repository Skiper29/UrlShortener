using FluentAssertions;
using UrlShortener.BLL.Services;

namespace UrlShortener.UnitTests.Services;

public class UrlShortenerServiceTests
{
    private readonly UrlShortenerService _sut = new();

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(999)]
    public void GenerateShortCode_ForPositiveId_ReturnsNonEmptyString(int id)
    {
        // Act
        var result = _sut.GenerateShortCode(id);

        // Assert
        result.Should().NotBeNullOrEmpty();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(62)]
    [InlineData(3844)]
    public void GenerateShortCode_SameIdCalledTwice_ReturnsSameCode(int id)
    {
        // Act
        var first = _sut.GenerateShortCode(id);
        var second = _sut.GenerateShortCode(id);

        // Assert
        first.Should().Be(second);
    }

    [Fact]
    public void GenerateShortCode_DifferentIds_ReturnDifferentCodes()
    {
        // Arrange
        var ids = Enumerable.Range(1, 100).ToList();

        // Act
        var codes = ids.Select(_sut.GenerateShortCode).ToList();

        // Assert
        codes.Should().OnlyHaveUniqueItems();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(1000)]
    [InlineData(100_000)]
    public void GenerateShortCode_ReturnsOnlyBase62Characters(int id)
    {
        // Arrange
        const string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        // Act
        var result = _sut.GenerateShortCode(id);

        // Assert
        result.Should().MatchRegex($"^[{alphabet}]+$");
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(62, 2)]
    [InlineData(3844, 3)]
    public void GenerateShortCode_LargerIds_ProduceLongerOrEqualCodes(int largeId, int smallId)
    {
        // Act
        var smallCode = _sut.GenerateShortCode(smallId);
        var largeCode = _sut.GenerateShortCode(largeId);

        // Assert
        largeCode.Length.Should().BeGreaterThanOrEqualTo(smallCode.Length);
    }
}
