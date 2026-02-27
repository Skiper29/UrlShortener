using FluentResults;
using MediatR;

namespace UrlShortener.BLL.Features.Urls.Commands.Delete;

public record DeleteShortUrlCommand(int Id, string UserId) : IRequest<Result<Unit>>;
