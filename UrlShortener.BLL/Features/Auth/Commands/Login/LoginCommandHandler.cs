using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UrlShortener.BLL.DTOs.Auth;
using UrlShortener.BLL.Interfaces;
using UrlShortener.DAL.Entities;

namespace UrlShortener.BLL.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtService _jwtService;

    public LoginCommandHandler(UserManager<AppUser> userManager, IJwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.LoginRequest.Login);
        if (user == null)
        {
            return Result.Fail("Invalid credentials.");
        }

        var passwordValid = await _userManager.CheckPasswordAsync(user, request.LoginRequest.Password);
        if (!passwordValid)
        {
            return Result.Fail("Invalid credentials.");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtService.GenerateToken(user, roles);

        var response = new AuthResponse(token, user.UserName!, user.Email!, roles.FirstOrDefault() ?? string.Empty);

        return Result.Ok(response);
    }
}
