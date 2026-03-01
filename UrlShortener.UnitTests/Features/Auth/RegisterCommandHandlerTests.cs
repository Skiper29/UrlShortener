using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using UrlShortener.BLL.Features.Auth.Commands.Register;
using UrlShortener.BLL.Interfaces;
using UrlShortener.DAL.Entities;
using UrlShortener.DAL.Enums;
using UrlShortener.UnitTests.Common;

namespace UrlShortener.UnitTests.Features.Auth;

public class RegisterCommandHandlerTests
{
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly Mock<IJwtService> _jwtServiceMock;
    private readonly RegisterCommandHandler _sut;

    public RegisterCommandHandlerTests()
    {
        _userManagerMock = TestDataBuilder.CreateUserManagerMock();
        _jwtServiceMock = new Mock<IJwtService>();

        _sut = new RegisterCommandHandler(
            _userManagerMock.Object,
            _jwtServiceMock.Object);
    }

    private static RegisterCommand CreateValidCommand() =>
       new(new("newuser", "new@example.com", "Password@123"));

    private void SetupSuccessfulRegistration(string token = "jwt.token")
    {
        _userManagerMock
            .Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock
            .Setup(x => x.AddToRoleAsync(It.IsAny<AppUser>(), AppRole.User.ToString()))
            .ReturnsAsync(IdentityResult.Success);

        _userManagerMock
            .Setup(x => x.GetRolesAsync(It.IsAny<AppUser>()))
            .ReturnsAsync(new List<string> { AppRole.User.ToString() });

        _jwtServiceMock
            .Setup(x => x.GenerateToken(It.IsAny<AppUser>(), It.IsAny<IList<string>>()))
            .Returns(token);
    }

    [Fact]
    public async Task Handle_ValidRegistration_ReturnsAuthResponse()
    {
        // Arrange
        var token = "new.jwt.token";
        SetupSuccessfulRegistration(token);
        var command = CreateValidCommand();

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Token.Should().Be(token);
        result.Value.UserName.Should().Be(command.RegisterRequest.UserName);
        result.Value.Email.Should().Be(command.RegisterRequest.Email);
        result.Value.Role.Should().Be(AppRole.User.ToString());
    }

    [Fact]
    public async Task Handle_ValidCommand_AssignsUserRoleNotAdmin()
    {
        // Arrange
        SetupSuccessfulRegistration();
        var command = CreateValidCommand();

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Role.Should().Be(AppRole.User.ToString());

        _userManagerMock.Verify(
            x => x.AddToRoleAsync(It.IsAny<AppUser>(), AppRole.User.ToString()),
            Times.Once);

        _userManagerMock.Verify(
            x => x.AddToRoleAsync(It.IsAny<AppUser>(), AppRole.Admin.ToString()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_IdentityFailure_FailsWithErrorMessage()
    {
        // Arrange
        var errors = new[] { new IdentityError { Description = "Username already taken." } };

        _userManagerMock
            .Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(errors));

        var command = CreateValidCommand();

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be("Username already taken.");
    }

    [Fact]
    public async Task Handle_IdentityFailure_NeverCallsJwtService()
    {
        // Arrange
        _userManagerMock
            .Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error" }));

        var command = CreateValidCommand();

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsFailed.Should().BeTrue();

        _jwtServiceMock.Verify(
            x => x.GenerateToken(It.IsAny<AppUser>(), It.IsAny<IList<string>>()),
            Times.Never);
    }
}
