using System.Reflection;
using AuthService.Application.Behaviors;
using AuthService.Application.Cache;
using AuthService.Application.Commands;
using AuthService.Application.Commands.CreateUser;
using AuthService.Application.JwtConfig;
using AuthService.Application.Security;
using AuthService.DataContext.Database;
using AuthService.Domain.Models.TokenAggregate.Repos;
using AuthService.Domain.Models.UserAggregate.Repos;
using AuthService.Infrastructure.Cache;
using AuthService.Infrastructure.Extensions.Jwt;
using AuthService.Infrastructure.JwtGenerator;
using AuthService.Infrastructure.Repos;
using AuthService.Infrastructure.Security;
using FluentValidation;
using MediatR;
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

        builder.Services.AddScoped(
            typeof(IPipelineBehavior<,>),
        typeof(ValidationPipelineBehavior<,>));
        
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        return builder;
    }

    public static WebApplicationBuilder AddCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(corsOptions =>
        {
            corsOptions.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

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