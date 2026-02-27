using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UrlShortener.BLL.DTOs.Urls;
using UrlShortener.DAL.Entities;
using UrlShortener.DAL.Repositories.Interfaces.Base;
using UrlShortener.DAL.Repositories.Options;

namespace UrlShortener.BLL.Features.Urls.Queries.GetById;

public class GetUrlByIdQueryHandler : IRequestHandler<GetUrlByIdQuery, Result<UrlDetailResponse>>
{
    private readonly IRepositoryWrapper _repository;
    private readonly IMapper _mapper;

    public GetUrlByIdQueryHandler(IRepositoryWrapper repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<UrlDetailResponse>> Handle(GetUrlByIdQuery request, CancellationToken cancellationToken)
    {
        var url = await _repository.UrlRepository.GetFirstOrDefaultAsync(new QueryOptions<ShortenedUrl>
        {
            Filter = u => u.Id == request.Id,
            Include = q => q.Include(u => u.CreatedBy)

        });

        if (url == null)
        {
            return Result.Fail<UrlDetailResponse>("URL not found.");
        }

        var dto = _mapper.Map<UrlDetailResponse>(url);
        return Result.Ok(dto with { ShortUrl = $"/r/{url.ShortCode}" });
    }
}
