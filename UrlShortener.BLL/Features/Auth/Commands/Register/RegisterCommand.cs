using FluentResults;
using MediatR;
using UrlShortener.BLL.DTOs.Auth;

namespace UrlShortener.BLL.Features.Auth.Commands.Register;

public record RegisterCommand(RegisterRequest RegisterRequest) : IRequest<Result<AuthResponse>>;
