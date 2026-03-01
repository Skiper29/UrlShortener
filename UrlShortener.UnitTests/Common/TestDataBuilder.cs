using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using UrlShortener.DAL.Entities;

namespace UrlShortener.UnitTests.Common;

public static class TestDataBuilder
{
    // Users
    public static AppUser CreateUser(
        string id = "user-1",
        string userName = "testuser",
        string email = "test@example.com") => new()
        {
            Id = id,
            UserName = userName,
            Email = email,
            EmailConfirmed = true,
        };

    public static AppUser CreateAdmin(
        string id = "admin-1",
        string userName = "admin",
        string email = "admin@example.com") => new()
        {
            Id = id,
            UserName = userName,
            Email = email,
            EmailConfirmed = true,
        };

    // ShortenedUrls
    public static ShortenedUrl CreateShortenedUrl(
        int id = 1,
        string originalUrl = "https://example.com",
        string shortCode = "abc123",
        string createdById = "user-1",
        AppUser? createdBy = null) => new()
        {
            Id = id,
            OriginalUrl = originalUrl,
            ShortCode = shortCode,
            CreatedById = createdById,
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            CreatedBy = createdBy ?? CreateUser(id: createdById),
        };

    public static List<ShortenedUrl> CreateShortenedUrlList(int count = 3)
    {
        var user = CreateUser();
        return Enumerable.Range(1, count)
            .Select(i => CreateShortenedUrl(
                id: i,
                originalUrl: $"https://example{i}.com",
                shortCode: $"code{i}",
                createdById: user.Id,
                createdBy: user))
            .ToList();
    }

    // Identity Mocks
    public static Mock<UserManager<AppUser>> CreateUserManagerMock()
    {
        var store = new Mock<IUserStore<AppUser>>();
        return new Mock<UserManager<AppUser>>(
            store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
    }

    public static Mock<SignInManager<AppUser>> CreateSignInManagerMock(
        Mock<UserManager<AppUser>> userManagerMock)
    {
        var contextAccessor = new Mock<IHttpContextAccessor>();
        var claimsFactory = new Mock<IUserClaimsPrincipalFactory<AppUser>>();
        return new Mock<SignInManager<AppUser>>(
            userManagerMock.Object,
            contextAccessor.Object,
            claimsFactory.Object,
            null!, null!, null!, null!);
    }
}
