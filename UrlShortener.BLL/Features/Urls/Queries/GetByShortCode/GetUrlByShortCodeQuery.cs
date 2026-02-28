using FluentResults;
using MediatR;
using UrlShortener.BLL.DTOs.Urls;

namespace UrlShortener.BLL.Features.Urls.Queries.GetByShortCode;

public record GetUrlByShortCodeQuery(string ShortCode) : IRequest<Result<UrlDetailResponse>>;
