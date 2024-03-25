using AuthService.DataContext.Database;
using AuthService.Domain.Models;
using AuthService.Domain.Models.UserAggregate.Repos;
using AuthService.Infrastructure.Extensions;
using AuthService.Infrastructure.Extensions.UsersSeed;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AuthService.Infrastructure.Middlewares;

public class UserSeedMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _provider;
    private readonly ILogger<UserSeedMiddleware> _logger;
    public static bool IsInitial = true;

    public UserSeedMiddleware(
        RequestDelegate next, 
        IServiceProvider provider, 
        ILogger<UserSeedMiddleware> logger)
    {
        _next = next;
        _provider = provider;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (IsInitial)
        {
            using var scope = _provider.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                //Seed Default Users
                var repo = services.GetRequiredService<IUserRepository>();
                var db = services.GetRequiredService<UserDb>();
                await SeedExtensions.SeedEssentialsAsync(repo, db);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred seeding the DB.");
                IsInitial = false;
            }
        }

        await _next(context);
    }
}

public static class UserSeedMiddlewareExtension 
{
    /// <summary>
    /// Only for development
    /// </summary>
    /// <returns></returns>
    public static IApplicationBuilder UseSeedUsers(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<UserSeedMiddleware>();
        return builder;
    }
}