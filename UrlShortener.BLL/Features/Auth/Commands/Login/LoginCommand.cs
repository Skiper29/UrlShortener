using FluentResults;
using MediatR;
using UrlShortener.BLL.DTOs.Auth;

namespace UrlShortener.BLL.Features.Auth.Commands.Login;

public record LoginCommand(LoginRequest LoginRequest) : IRequest<Result<AuthResponse>>;
