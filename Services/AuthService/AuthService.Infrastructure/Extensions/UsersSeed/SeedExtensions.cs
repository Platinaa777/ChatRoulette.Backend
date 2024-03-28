using AuthService.DataContext.Database;
using AuthService.DataContext.Utils;
using AuthService.Domain.Models.UserAggregate;
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

        var passwordHasher = new Hasher();

        var salt1 = new Salt(passwordHasher.GenerateSalt());
        var hashedPassword1 = Password.Create(passwordHasher.HashPasswordWithSalt("admin123", salt1.Value)).Value;
        
        var admin = new User(
            id: Guid.NewGuid().ToString(),
            Name.Create("denis").Value,
            Email.Create("m@edu.hse.ru").Value,
            Name.Create("platina777").Value,
            Age.Create(19).Value,
            hashedPassword1,
            salt1,
            RoleType.Admin);
        admin.SubmitEmail();

        await repository.AddUserAsync(admin);
        await context.SaveChangesAsync();
    }

    public static WebApplicationBuilder AddUsersSeed(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IStartupFilter, UserSeedFilter>();
        return builder;
    }
}