using AutoMapper;
using FluentAssertions;
using Moq;
using UrlShortener.BLL.DTOs.Urls;
using UrlShortener.BLL.Features.Urls.Commands.Create;
using UrlShortener.BLL.Interfaces;
using UrlShortener.DAL.Entities;
using UrlShortener.DAL.Repositories.Interfaces;
using UrlShortener.DAL.Repositories.Interfaces.Base;
using UrlShortener.DAL.Repositories.Options;
using UrlShortener.UnitTests.Common;

namespace UrlShortener.UnitTests.Features.Urls;

public class CreateShortUrlCommandHandlerTests
{
    private readonly Mock<IRepositoryWrapper> _repositoryWrapperMock;
    private readonly Mock<IUrlShortenerService> _urlShortenerServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUrlRepository> _urlRepositoryMock;
    private readonly CreateShortUrlCommandHandler _sut;

    public CreateShortUrlCommandHandlerTests()
    {
        _urlShortenerServiceMock = new Mock<IUrlShortenerService>();
        _mapperMock = new Mock<IMapper>();
        _urlRepositoryMock = new Mock<IUrlRepository>();
        _repositoryWrapperMock = new Mock<IRepositoryWrapper>();

        _repositoryWrapperMock.Setup(x => x.UrlRepository).Returns(_urlRepositoryMock.Object);

        _sut = new CreateShortUrlCommandHandler(
            _urlShortenerServiceMock.Object,
            _repositoryWrapperMock.Object,
            _mapperMock.Object);
    }

    private static CreateShortUrlCommand CreateValidCommand(
        string url = "https://example.com",
        string userId = "user-1",
        string userName = "user") => new(new(url), userId, userName);

    private void SetupSuccessfulCreation(ShortenedUrl entity, string shortCode = "abc")
    {
        _urlRepositoryMock
            .Setup(x => x.GetFirstOrDefaultAsync(It.IsAny<QueryOptions<ShortenedUrl>>()))
            .ReturnsAsync((ShortenedUrl?)null);

        _urlRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<ShortenedUrl>()))
            .ReturnsAsync(entity);

        _repositoryWrapperMock
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        _urlShortenerServiceMock
            .Setup(x => x.GenerateShortCode(entity.Id))
            .Returns(shortCode);

        _mapperMock
            .Setup(x => x.Map<UrlResponse>(It.IsAny<ShortenedUrl>()))
            .Returns(new UrlResponse { OriginalUrl = entity.OriginalUrl, ShortCode = shortCode });
    }

    [Fact]
    public async Task Handle_NewUrl_ReturnsCreatedUrlResponse()
    {
        // Arrange
        var entity = TestDataBuilder.CreateShortenedUrl();
        SetupSuccessfulCreation(entity, "xyz");
        var command = CreateValidCommand(entity.OriginalUrl);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.OriginalUrl.Should().Be(entity.OriginalUrl);
        result.Value.ShortCode.Should().Be("xyz");
    }

    [Fact]
    public async Task Handle_DuplicateUrl_FailsWithErrorMessage()
    {
        // Arrange
        var existingEntity = TestDataBuilder.CreateShortenedUrl();
        _urlRepositoryMock
            .Setup(x => x.GetFirstOrDefaultAsync(It.IsAny<QueryOptions<ShortenedUrl>>()))
            .ReturnsAsync(existingEntity);
        var command = CreateValidCommand(existingEntity.OriginalUrl);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Message == "This URL already exists.");
    }

    [Fact]
    public async Task Handle_DuplicateUrl_NeverCallsCreateAsync()
    {
        // Arrange
        var existingEntity = TestDataBuilder.CreateShortenedUrl();
        _urlRepositoryMock
            .Setup(x => x.GetFirstOrDefaultAsync(It.IsAny<QueryOptions<ShortenedUrl>>()))
            .ReturnsAsync(existingEntity);
        var command = CreateValidCommand(existingEntity.OriginalUrl);

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        _urlRepositoryMock.Verify(
            x => x.CreateAsync(It.IsAny<ShortenedUrl>()),
            Times.Never);
    }
}
