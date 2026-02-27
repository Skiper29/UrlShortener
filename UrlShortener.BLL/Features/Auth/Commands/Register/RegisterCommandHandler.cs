using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UrlShortener.BLL.DTOs.Auth;
using UrlShortener.BLL.Interfaces;
using UrlShortener.DAL.Entities;
using UrlShortener.DAL.Enums;

namespace UrlShortener.BLL.Features.Auth.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AuthResponse>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IJwtService _jwtService;

    public RegisterCommandHandler(UserManager<AppUser> userManager, IJwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public async Task<Result<AuthResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new AppUser
        {
            UserName = request.RegisterRequest.UserName,
            Email = request.RegisterRequest.Email,
            EmailConfirmed = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.RegisterRequest.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToArray();
            return Result.Fail<AuthResponse>(string.Join(", ", errors));
        }

        await _userManager.AddToRoleAsync(user, AppRole.User.ToString());

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtService.GenerateToken(user, roles);

        var authResponse = new AuthResponse(token, user.UserName, user.Email, roles.FirstOrDefault() ?? string.Empty);

        return Result.Ok(authResponse);
    }
}
