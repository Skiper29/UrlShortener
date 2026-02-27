using FluentResults;
using MediatR;
using UrlShortener.BLL.DTOs.Urls;

namespace UrlShortener.BLL.Features.Urls.Queries.GetById;

public record GetUrlByIdQuery(int Id) : IRequest<Result<UrlDetailResponse>>;
