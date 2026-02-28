using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.BLL.DTOs.Urls;
using UrlShortener.DAL.Entities;
using UrlShortener.DAL.Repositories.Interfaces.Base;
using UrlShortener.DAL.Repositories.Options;

namespace UrlShortener.BLL.Features.Urls.Queries.GetAll;

public class GetAllUrlsQueryHandler : IRequestHandler<GetAllUrlsQuery, Result<IEnumerable<UrlResponse>>>
{
    private readonly IRepositoryWrapper _repository;
    private readonly IMapper _mapper;

    public GetAllUrlsQueryHandler(IRepositoryWrapper repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<UrlResponse>>> Handle(GetAllUrlsQuery request, CancellationToken cancellationToken)
    {
        var urls = await _repository.UrlRepository.GetAllAsync(new QueryOptions<ShortenedUrl>
        {
            Include = q => q.Include(u => u.CreatedBy),
        });

        return Result.Ok(urls.Select(url =>
        {
            var dto = _mapper.Map<UrlResponse>(url);
            return dto with { ShortUrl = $"/r/{url.ShortCode}" };
        }));
    }
}
