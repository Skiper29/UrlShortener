using AutoMapper;
using FluentResults;
using MediatR;
using UrlShortener.BLL.DTOs.Urls;
using UrlShortener.BLL.Interfaces;
using UrlShortener.DAL.Entities;
using UrlShortener.DAL.Repositories.Interfaces.Base;
using UrlShortener.DAL.Repositories.Options;

namespace UrlShortener.BLL.Features.Urls.Commands.Create;

public class CreateShortUrlCommandHandler : IRequestHandler<CreateShortUrlCommand, Result<UrlResponse>>
{
    private readonly IUrlShortenerService _urlShortenerService;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;

    public CreateShortUrlCommandHandler(IUrlShortenerService urlShortenerService, IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _urlShortenerService = urlShortenerService;
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public async Task<Result<UrlResponse>> Handle(CreateShortUrlCommand request, CancellationToken cancellationToken)
    {
        var exisitingUrlResult = await _repositoryWrapper.UrlRepository.GetFirstOrDefaultAsync(new QueryOptions<ShortenedUrl>
        {
            Filter = u => u.OriginalUrl == request.CreateUrlRequest.OriginalUrl
        });

        if (exisitingUrlResult is not null)
        {
            return Result.Fail("This URL already exists.");
        }

        var entity = new ShortenedUrl
        {
            OriginalUrl = request.CreateUrlRequest.OriginalUrl,
            ShortCode = string.Empty,
            CreatedById = request.UserId,
            CreatedAt = DateTime.UtcNow
        };

        var createdEntity = await _repositoryWrapper.UrlRepository.CreateAsync(entity);

        await _repositoryWrapper.SaveChangesAsync();

        createdEntity.ShortCode = _urlShortenerService.GenerateShortCode(createdEntity.Id);

        await _repositoryWrapper.SaveChangesAsync();

        var response = _mapper.Map<UrlResponse>(createdEntity);

        return Result.Ok(response with
        {
            ShortUrl = $"/r/{createdEntity.ShortCode}",
            CreatedBy = request.UserName
        });
    }
}
