using FluentResults;
using MediatR;
using UrlShortener.BLL.DTOs.About;

namespace UrlShortener.BLL.Features.About.Commands.Update;

public record UpdateAboutContentCommand(AboutContentUpdateRequest UpdateRequest) : IRequest<Result<AboutContentResponse>>;
