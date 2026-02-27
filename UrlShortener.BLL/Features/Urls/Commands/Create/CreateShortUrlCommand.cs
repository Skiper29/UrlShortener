using FluentResults;
using MediatR;
using UrlShortener.BLL.DTOs.Urls;

namespace UrlShortener.BLL.Features.Urls.Commands.Create;

public record CreateShortUrlCommand(CreateUrlRequest CreateUrlRequest, string UserId) : IRequest<Result<UrlResponse>>;
