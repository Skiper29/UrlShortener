using AutoMapper;
using FluentResults;
using MediatR;
using UrlShortener.BLL.DTOs.About;
using UrlShortener.DAL.Repositories.Interfaces.Base;

namespace UrlShortener.BLL.Features.About.Queries.Get;

public class GetAboutContentQueryHandler : IRequestHandler<GetAboutContentQuery, Result<AboutContentResponse>>
{
    private readonly IRepositoryWrapper _repository;
    private readonly IMapper _mapper;

    public GetAboutContentQueryHandler(IRepositoryWrapper repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<AboutContentResponse>> Handle(GetAboutContentQuery request, CancellationToken cancellationToken)
    {
        var aboutContent = await _repository.AboutContentRepository.GetFirstOrDefaultAsync();

        if (aboutContent == null)
        {
            return Result.Fail("About content not found.");
        }

        return Result.Ok(_mapper.Map<AboutContentResponse>(aboutContent));
    }
}
