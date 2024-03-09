using AuthService.DataContext.Database;
using AuthService.DataContext.Utils;
using AuthService.Domain.Models.UserAggregate.Entities;
using AuthService.Domain.Models.UserAggregate.Enumerations;
using AuthService.Domain.Models.UserAggregate.Repos;
using AuthService.Domain.Models.UserAggregate.ValueObjects;
using AuthService.Domain.Models.UserAggregate.ValueObjects.User;
using AuthService.Infrastructure.Filters;
using AuthService.Infrastructure.Security;
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

        var passwordHasher = new Hasher();

        var salt1 = new Salt(passwordHasher.GenerateSalt());
        var hashedPassword1 = new Password(passwordHasher.HashPasswordWithSalt("admin123", salt1.Value));
        
        var admin = new User(
            id: Guid.NewGuid().ToString(),
            new Name("denis"),
            new Email("m@edu.hse.ru"),
            new Name("platina777"),
            new Age(19),
            hashedPassword1,
            salt1,
            RoleType.Admin);
        admin.SubmitEmail();

        await repository.AddUserAsync(admin);
    }

    public static WebApplicationBuilder AddUsersSeed(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IStartupFilter, UserSeedFilter>();
        return builder;
    }
}