using FluentAssertions;
using Moq;
using UrlShortener.BLL.Features.Urls.Commands.Delete;
using UrlShortener.DAL.Entities;
using UrlShortener.DAL.Repositories.Interfaces;
using UrlShortener.DAL.Repositories.Interfaces.Base;
using UrlShortener.DAL.Repositories.Options;
using UrlShortener.UnitTests.Common;

namespace UrlShortener.UnitTests.Features.Urls;

public class DeleteShortUrlCommandHandlerTests
{
    private readonly Mock<IRepositoryWrapper> _repositoryWrapperMock;
    private readonly Mock<IUrlRepository> _urlRepositoryMock;
    private readonly DeleteShortUrlCommandHandler _sut;

    public DeleteShortUrlCommandHandlerTests()
    {
        _urlRepositoryMock = new Mock<IUrlRepository>();
        _repositoryWrapperMock = new Mock<IRepositoryWrapper>();

        _repositoryWrapperMock.Setup(x => x.UrlRepository).Returns(_urlRepositoryMock.Object);

        _sut = new DeleteShortUrlCommandHandler(
            _repositoryWrapperMock.Object);
    }

    private void SetupUrlExists(int id = 1, string ownerId = "user-1")
    {
        var url = TestDataBuilder.CreateShortenedUrl(id: id, createdById: ownerId);

        _urlRepositoryMock
            .Setup(x => x.GetFirstOrDefaultAsync(It.IsAny<QueryOptions<ShortenedUrl>>()))
            .ReturnsAsync(url);

        _urlRepositoryMock
            .Setup(x => x.Delete(url));

        _repositoryWrapperMock
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);
    }

    [Fact]
    public async Task Handle_OwnerDeletesOwnUrl_ReturnsTrue()
    {
        // Arrange
        SetupUrlExists(id: 1, ownerId: "user-1");
        var command = new DeleteShortUrlCommand(1, "user-1", IsAdmin: false);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_OwnerDeletesOwnUrl_CallsDeleteAsync()
    {
        // Arrange
        SetupUrlExists(id: 1, ownerId: "user-1");
        var command = new DeleteShortUrlCommand(1, "user-1", IsAdmin: false);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        _urlRepositoryMock.Verify(
            x => x.Delete(It.Is<ShortenedUrl>(u => u.Id == 1)),
            Times.Once);
    }
    [Fact]
    public async Task Handle_AdminDeletesAnotherUsersUrl_ReturnsTrue()
    {
        // Arrange  
        SetupUrlExists(id: 1, ownerId: "user-1");
        var command = new DeleteShortUrlCommand(1, "admin-user", IsAdmin: true);

        // Act  
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_UserDeletesAnotherUsersUrl_Fails()
    {
        // Arrange  
        SetupUrlExists(id: 1, ownerId: "user-1");
        var command = new DeleteShortUrlCommand(1, "user-2", IsAdmin: false);

        // Act  
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_UserDeletesAnotherUsersUrl_NeverCallsDeleteAsync()
    {
        // Arrange  
        SetupUrlExists(id: 1, ownerId: "user-1");
        var command = new DeleteShortUrlCommand(1, "user-2", IsAdmin: false);

        // Act  
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        _urlRepositoryMock.Verify(
            x => x.Delete(It.IsAny<ShortenedUrl>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_UrlNotFound_FailsWithNotFound()
    {
        // Arrange  
        _urlRepositoryMock
            .Setup(x => x.GetFirstOrDefaultAsync(It.IsAny<QueryOptions<ShortenedUrl>>()))
            .ReturnsAsync((ShortenedUrl?)null);
        var command = new DeleteShortUrlCommand(1, "user-1", IsAdmin: false);

        // Act  
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle(e => e.Message.Contains("URL not found."));
    }
}
