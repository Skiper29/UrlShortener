using FluentResults;
using MediatR;
using UrlShortener.BLL.DTOs.About;

namespace UrlShortener.BLL.Features.About.Queries.Get;

public record GetAboutContentQuery() : IRequest<Result<AboutContentResponse>>;
