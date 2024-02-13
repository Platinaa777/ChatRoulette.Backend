using AuthService.Domain.Models;
using AuthService.Infrastructure.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Infrastructure.Extensions;

public static class SeedExtensions
{
    public static async Task SeedEssentialsAsync(UserManager<UserAccount> userManager, RoleManager<IdentityRole> roleManager)
    {
        //Seed Roles
        await roleManager.CreateAsync(new IdentityRole(Role.Admin.Name));
        await roleManager.CreateAsync(new IdentityRole(Role.ActivatedUser.Name));
        await roleManager.CreateAsync(new IdentityRole(Role.UnactivatedUser.Name));
        
        var admin = new UserAccount()
        {
            UserName = "platina",
            Age = 19,
            Email = "mir@edu.hse.ru",
        };
        
        await userManager.CreateAsync(admin, "admin123");
        await userManager.AddToRoleAsync(admin, Role.Admin.Name);
        
        var active = new UserAccount()
        {
            UserName = "gamer",
            Age = 19,
            Email = "vovchik@edu.hse.ru",
        };
        
        await userManager.CreateAsync(active, "active123");
        await userManager.AddToRoleAsync(active, Role.ActivatedUser.Name);
        
        var inactive = new UserAccount()
        {
            UserName = "pony",
            Age = 22,
            Email = "alexander@edu.hse.ru",
        };
        
        var res = await userManager.CreateAsync(inactive, "inactive123");
        await userManager.AddToRoleAsync(inactive, Role.UnactivatedUser.Name);
    }

    public static WebApplicationBuilder AddUsersSeed(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IStartupFilter, UserSeedFilter>();
        return builder;
    }
}