using AuthService.DataContext.Database;
using AuthService.DataContext.Utils;
using AuthService.Domain.Models.UserAggregate.Entities;
using AuthService.Domain.Models.UserAggregate.Enumerations;
using AuthService.Domain.Models.UserAggregate.Repos;
using AuthService.Domain.Models.UserAggregate.ValueObjects;
using AuthService.Infrastructure.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Infrastructure.Extensions.UsersSeed;

public static class SeedExtensions
{
    public static async Task SeedEssentialsAsync(IUserRepository repository, UserDb context)
    {
        if (await DatabaseChecker.IsDbEmpty(context))
            return;
        
        var admin = new User(
            id: Guid.NewGuid().ToString(),
            new Name("denis"),
            new Email("m@edu.hse.ru"),
            new Name("platina777"),
            new Age(19),
            new Password("admin123"),
            RoleType.Admin);
        
        
        var activeUser = new User(
            id: Guid.NewGuid().ToString(),
            new Name("petya"),
            new Email("p@edu.hse.ru"),
            new Name("gamer228"),
            new Age(19),
            new Password("active123"),
            RoleType.ActivatedUser);
        
        var inactiveUser = new User(
            id: Guid.NewGuid().ToString(),
            new Name("vova"),
            new Email("v@edu.hse.ru"),
            new Name("vovantus"),
            new Age(19),
            new Password("inactive123"),
            RoleType.UnactivatedUser);

        await repository.AddUserAsync(admin);
        await repository.AddUserAsync(activeUser);
        await repository.AddUserAsync(inactiveUser);
    }

    public static WebApplicationBuilder AddUsersSeed(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IStartupFilter, UserSeedFilter>();
        return builder;
    }
}