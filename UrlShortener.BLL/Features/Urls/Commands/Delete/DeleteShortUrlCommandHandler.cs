using FluentResults;
using MediatR;
using UrlShortener.DAL.Entities;
using UrlShortener.DAL.Repositories.Interfaces.Base;
using UrlShortener.DAL.Repositories.Options;

namespace UrlShortener.BLL.Features.Urls.Commands.Delete;

public class DeleteShortUrlCommandHandler : IRequestHandler<DeleteShortUrlCommand, Result<Unit>>
{
    private readonly IRepositoryWrapper _repository;

    public DeleteShortUrlCommandHandler(IRepositoryWrapper repository)
    {
        _repository = repository;
    }

    public async Task<Result<Unit>> Handle(DeleteShortUrlCommand request, CancellationToken cancellationToken)
    {
        var url = await _repository.UrlRepository.GetFirstOrDefaultAsync(new QueryOptions<ShortenedUrl>
        {
            Filter = u => u.Id == request.Id,
        });

        if (url is null)
        {
            return Result.Fail("URL not found.");
        }


        if (!request.IsAdmin && url.CreatedById != request.UserId)
        {
            return Result.Fail("You do not have permission to delete this URL.");
        }

        _repository.UrlRepository.Delete(url);

        var result = await _repository.SaveChangesAsync();

        if (result > 0)
        {
            return Result.Ok();
        }

        return Result.Fail("Failed to delete the URL.");
    }
}
