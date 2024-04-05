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
        
        var admin = User.Create(
            Guid.NewGuid().ToString(),
            "denis",
            "m@edu.hse.ru",
            "platina777",
            19,
            hashedPassword1.Value,
            salt1.Value,
            RoleType.Admin.Name).Value;
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