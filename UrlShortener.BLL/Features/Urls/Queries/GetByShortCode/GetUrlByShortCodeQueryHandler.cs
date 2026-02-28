using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.BLL.DTOs.Urls;
using UrlShortener.DAL.Entities;
using UrlShortener.DAL.Repositories.Interfaces.Base;
using UrlShortener.DAL.Repositories.Options;

namespace UrlShortener.BLL.Features.Urls.Queries.GetByShortCode;

public class GetUrlByShortCodeQueryHandler : IRequestHandler<GetUrlByShortCodeQuery, Result<UrlDetailResponse>>
{
    private readonly IRepositoryWrapper _repository;
    private readonly IMapper _mapper;

    public GetUrlByShortCodeQueryHandler(IRepositoryWrapper repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<UrlDetailResponse>> Handle(GetUrlByShortCodeQuery request, CancellationToken cancellationToken)
    {
        var url = await _repository.UrlRepository.GetFirstOrDefaultAsync(new QueryOptions<ShortenedUrl>
        {
            Filter = u => u.ShortCode == request.ShortCode,
            Include = q => q.Include(u => u.CreatedBy)
        });

        if (url is null)
        {
            return Result.Fail<UrlDetailResponse>("URL not found.");
        }

        var dto = _mapper.Map<UrlDetailResponse>(url);
        return Result.Ok(dto with { ShortUrl = $"/r/{url.ShortCode}" });
    }
}
