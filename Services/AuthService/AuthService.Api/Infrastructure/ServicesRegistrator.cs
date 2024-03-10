using AuthService.Application.Cache;
using AuthService.Application.Commands;
using AuthService.Application.Commands.CreateUser;
using AuthService.Application.Security;
using AuthService.DataContext.Database;
using AuthService.Domain.JwtConfig;
using AuthService.Domain.Models.UserAggregate.Repos;
using AuthService.Infrastructure.Cache;
using AuthService.Infrastructure.Extensions.Jwt;
using AuthService.Infrastructure.JwtGenerator;
using AuthService.Infrastructure.Repos;
using AuthService.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace AuthService.Api.Infrastructure;

public static class ServicesRegistrator
{
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ITokenRepository, RefreshTokenRepository>();

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
        
        builder.Services.AddScoped<IJwtManager, JwtTokenCreator>();
        builder.Services.AddScoped<IHasherPassword, Hasher>();
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
        
        builder.Services.AddScoped<ICacheStorage, RedisCache>();

        return builder;
    }
}