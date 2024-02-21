using AuthService.DataContext.Database;
using AuthService.DataContext.Utils;
using AuthService.Domain.Models.UserAggregate.Entities;
using AuthService.Domain.Models.UserAggregate.Enumerations;
using AuthService.Domain.Models.UserAggregate.Repos;
using AuthService.Domain.Models.UserAggregate.ValueObjects;
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

        var salt1 = new Salt(PasswordHasher.GenerateSalt());
        var hashedPassword1 = new Password(PasswordHasher.HashPasswordWithSalt("admin123", salt1.Value));
        
        var admin = new User(
            id: Guid.NewGuid().ToString(),
            new Name("denis"),
            new Email("m@edu.hse.ru"),
            new Name("platina777"),
            new Age(19),
            hashedPassword1,
            salt1,
            RoleType.Admin);
        
        var salt2 = new Salt(PasswordHasher.GenerateSalt());
        var hashedPassword2 = new Password(PasswordHasher.HashPasswordWithSalt("active123", salt2.Value));
        
        var activeUser = new User(
            id: Guid.NewGuid().ToString(),
            new Name("petya"),
            new Email("p@edu.hse.ru"),
            new Name("gamer228"),
            new Age(19),
            hashedPassword2,
            salt2,
            RoleType.ActivatedUser);
        
        var salt3 = new Salt(PasswordHasher.GenerateSalt());
        var hashedPassword3 = new Password(PasswordHasher.HashPasswordWithSalt("inactive123", salt3.Value));
        
        var inactiveUser = new User(
            id: Guid.NewGuid().ToString(),
            new Name("vova"),
            new Email("v@edu.hse.ru"),
            new Name("vovantus"),
            new Age(19),
            hashedPassword3,
            salt3,
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