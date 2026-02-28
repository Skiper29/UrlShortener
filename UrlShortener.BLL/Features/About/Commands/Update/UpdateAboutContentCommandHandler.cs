using AutoMapper;
using FluentResults;
using MediatR;
using UrlShortener.BLL.DTOs.About;
using UrlShortener.DAL.Repositories.Interfaces.Base;

namespace UrlShortener.BLL.Features.About.Commands.Update;

public class UpdateAboutContentCommandHandler : IRequestHandler<UpdateAboutContentCommand, Result<AboutContentResponse>>
{
    private readonly IRepositoryWrapper _repository;
    private readonly IMapper _mapper;

    public UpdateAboutContentCommandHandler(IRepositoryWrapper repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<AboutContentResponse>> Handle(UpdateAboutContentCommand request, CancellationToken cancellationToken)
    {
        var aboutContent = await _repository.AboutContentRepository.GetFirstOrDefaultAsync();
        if (aboutContent == null)
        {
            return Result.Fail("About content not found.");
        }

        aboutContent.Content = request.UpdateRequest.Content;
        aboutContent.UpdatedAt = DateTime.UtcNow;

        _repository.AboutContentRepository.Update(aboutContent);

        var result = await _repository.SaveChangesAsync();

        if (result <= 0)
        {
            return Result.Fail("Failed to update about content.");
        }

        var response = _mapper.Map<AboutContentResponse>(aboutContent);
        return Result.Ok(response);
    }
}
