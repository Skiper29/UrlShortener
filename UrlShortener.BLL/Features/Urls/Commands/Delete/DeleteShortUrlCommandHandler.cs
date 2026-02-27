using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UrlShortener.DAL.Entities;
using UrlShortener.DAL.Enums;
using UrlShortener.DAL.Repositories.Interfaces.Base;
using UrlShortener.DAL.Repositories.Options;

namespace UrlShortener.BLL.Features.Urls.Commands.Delete;

public class DeleteShortUrlCommandHandler : IRequestHandler<DeleteShortUrlCommand, Result<Unit>>
{
    private readonly IRepositoryWrapper _repository;
    private readonly UserManager<AppUser> _userManager;

    public DeleteShortUrlCommandHandler(IRepositoryWrapper repository, UserManager<AppUser> userManager)
    {
        _repository = repository;
        _userManager = userManager;
    }

    public async Task<Result<Unit>> Handle(DeleteShortUrlCommand request, CancellationToken cancellationToken)
    {
        var url = await _repository.UrlRepository.GetFirstOrDefaultAsync(new QueryOptions<ShortenedUrl>
        {
            Filter = u => u.Id == request.Id,
            Include = u => u.Include(u => u.CreatedBy)
        });

        if (url is null)
        {
            return Result.Fail("URL not found.");
        }

        var user = await _userManager.FindByIdAsync(request.UserId);

        if (user is null)
        {
            return Result.Fail("User not found.");
        }

        var isAdmin = await _userManager.IsInRoleAsync(user, AppRole.Admin.ToString());

        if (!isAdmin && url.CreatedById != request.UserId)
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
