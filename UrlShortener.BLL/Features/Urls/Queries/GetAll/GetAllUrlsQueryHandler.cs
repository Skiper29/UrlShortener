using AutoMapper;
using FluentResults;
using MediatR;
using UrlShortener.BLL.DTOs.Urls;
using UrlShortener.DAL.Repositories.Interfaces.Base;

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
        var urls = await _repository.UrlRepository.GetAllAsync();

        return Result.Ok(urls.Select(url =>
        {
            var dto = _mapper.Map<UrlResponse>(url);
            return dto with { ShortUrl = $"/r/{url.ShortCode}" };
        }));
    }
}
