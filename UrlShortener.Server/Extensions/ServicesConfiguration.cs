using Microsoft.AspNetCore.Identity;
using UrlShortener.DAL.Entities;
using UrlShortener.DAL.Enums;

namespace UrlShortener.Server.Extensions;

public static class ServicesConfiguration
{
    public static async Task CreateInitialDataAsync(this WebApplication app)
    {
        await app.AddRoles();
        await app.CreateInitialAdminAsync();
    }

    public static async Task AddRoles(this WebApplication app)
    {
        await using var asyncServiceScope = app.Services.CreateAsyncScope();
        var roleManager = asyncServiceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        if (!roleManager.RoleExistsAsync(nameof(AppRole.Admin)).GetAwaiter().GetResult())
        {
            await roleManager.CreateAsync(new IdentityRole(nameof(AppRole.Admin)));
            await roleManager.CreateAsync(new IdentityRole(nameof(AppRole.User)));
        }
    }

    private static async Task CreateInitialAdminAsync(this WebApplication app)
    {
        await using var asyncServiceScope = app.Services.CreateAsyncScope();
        var userManager = asyncServiceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        var initialAdminEmail = app.Configuration["AdminCredentials:Email"]!;
        var adminPassword = app.Configuration["AdminCredentials:Password"] ?? throw new InvalidOperationException("Initial Admin Password secret variable is required");

        if (!initialAdminEmail.Contains('@'))
        {
            throw new InvalidOperationException("Initial Admin Email must be a valid email address");
        }

        if (await userManager.FindByEmailAsync(initialAdminEmail) is null)
        {
            var adminUser = new AppUser
            {
                UserName = initialAdminEmail,
                Email = initialAdminEmail,
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to create initial admin: {errors}");
            }

            await userManager.AddToRoleAsync(adminUser, nameof(AppRole.Admin));
        }
    }
}
