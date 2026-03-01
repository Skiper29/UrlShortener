using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using UrlShortener.BLL.Features.Auth.Commands.Login;
using UrlShortener.BLL.Interfaces;
using UrlShortener.DAL.Entities;
using UrlShortener.UnitTests.Common;

namespace UrlShortener.UnitTests.Features.Auth;

public class LoginCommandHandlerTests
{
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly LoginCommandHandler _sut;

    public LoginCommandHandlerTests()
    {
        _userManagerMock = TestDataBuilder.CreateUserManagerMock();
        _jwtServiceMock = new Mock<IJwtService>();

        _sut = new LoginCommandHandler(
            _userManagerMock.Object,
            _jwtServiceMock.Object);
    }

    private static LoginCommand CreateValidCommand() =>
        new(new("test@example.com", "Password@123"));

    private void SetupSuccessfulLogin(AppUser user, IList<string> roles, string token)
    {
        _userManagerMock
            .Setup(x => x.FindByEmailAsync(user.Email!))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(x => x.CheckPasswordAsync(user, It.IsAny<string>()))
            .ReturnsAsync(true);

        _userManagerMock
            .Setup(x => x.GetRolesAsync(user))
            .ReturnsAsync(roles);

        _jwtServiceMock
            .Setup(x => x.GenerateToken(user, roles))
            .Returns(token);
    }

    [Fact]
    public async Task Handle_ValidCredentials_ReturnsAuthResponse()
    {
        // Arrange
        var user = TestDataBuilder.CreateUser();
        var roles = new List<string> { "User" };
        const string token = "valid.jwt.token";

        SetupSuccessfulLogin(user, roles, token);
        var command = CreateValidCommand();

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Token.Should().Be(token);
        result.Value.UserName.Should().Be(user.UserName);
        result.Value.Email.Should().Be(user.Email);
        result.Value.Role.Should().Be("User");
    }

    [Fact]
    public async Task Handle_UserNotFound_FailsWithInvalidCredentials()
    {
        // Arrange
        _userManagerMock
            .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((AppUser?)null);

        var command = CreateValidCommand();

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be("Invalid credentials.");
    }

    [Fact]
    public async Task Handle_WrongPassword_FailsWithInvalidCredentials()
    {
        // Arrange
        var user = TestDataBuilder.CreateUser();

        _userManagerMock
            .Setup(x => x.FindByEmailAsync(user.Email!))
            .ReturnsAsync(user);

        _userManagerMock
            .Setup(x => x.CheckPasswordAsync(user, It.IsAny<string>()))
            .ReturnsAsync(false);

        var command = CreateValidCommand();

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be("Invalid credentials.");
    }

    [Fact]
    public async Task Handle_ValidCredentials_CallsJwtServiceOnce()
    {
        // Arrange
        var user = TestDataBuilder.CreateUser();
        var roles = new List<string> { "User" };

        SetupSuccessfulLogin(user, roles, "valid.jwt.token");
        var command = CreateValidCommand();

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        _jwtServiceMock.Verify(
            x => x.GenerateToken(user, roles),
            Times.Once);
    }
}
