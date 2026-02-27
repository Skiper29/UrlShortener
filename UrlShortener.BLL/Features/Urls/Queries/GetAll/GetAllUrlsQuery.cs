using FluentResults;
using MediatR;
using UrlShortener.BLL.DTOs.Urls;

namespace UrlShortener.BLL.Features.Urls.Queries.GetAll;

public record GetAllUrlsQuery() : IRequest<Result<IEnumerable<UrlResponse>>>;
