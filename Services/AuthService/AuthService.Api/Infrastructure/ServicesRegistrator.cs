using AuthService.Application.Handlers;
using AuthService.DataContext.Database;
using AuthService.Domain.JwtConfig;
using AuthService.Domain.Models.UserAggregate.Repos;
using AuthService.Infrastructure.Extensions.Jwt;
using AuthService.Infrastructure.JwtGenerator;
using AuthService.Infrastructure.Repos;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace AuthService.Api.Infrastructure;

public static class ServicesRegistrator
{
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddScoped<IUserRepository, UserRepository>();


        builder.Services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssemblyContaining<CreateUserCommandHandler>());

        return builder;
    }

    public static WebApplicationBuilder AddDataLayer(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<UserDb>(options =>
        {
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("PostgresSQL"),
                b => b.MigrationsAssembly("AuthService.Migrations"));
        });

        return builder;
    }

    public static WebApplicationBuilder AddSecurity(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        builder.Services.AddScoped<IPasswordRepository, PasswordRepository>();
        builder.Services.AddScoped<JwtTokenCreator>();
        builder.Services.Configure<Jwt>(configuration.GetSection("Jwt"));
        builder.AddJwtAuthentication(configuration.GetSection("Jwt:Key").Value!);

        return builder;
    }

    public static WebApplicationBuilder AddCacheUserConfirmation(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        builder.Services.AddStackExchangeRedisCache(setup =>
        {
            setup.Configuration = configuration["Redis:Host"];
        });

        return builder;
    }
}