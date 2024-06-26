using AuthService.Api.BackgroundJobs;
using AuthService.Api.Utils;
using AuthService.Application.Assembly;
using AuthService.Application.Behaviors;
using AuthService.Application.Cache;
using AuthService.Application.Commands.CreateUser;
using AuthService.Application.JwtConfig;
using AuthService.Application.Security;
using AuthService.DataContext.Database;
using AuthService.Domain.Models.TokenAggregate.Repos;
using AuthService.Domain.Models.UserAggregate.Repos;
using AuthService.Domain.Models.UserHistoryAggregate.Repos;
using AuthService.Infrastructure.Cache;
using AuthService.Infrastructure.Extensions.Jwt;
using AuthService.Infrastructure.JwtGenerator;
using AuthService.Infrastructure.Repos;
using AuthService.Infrastructure.Security;
using DomainDriverDesignAbstractions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Serilog;

namespace AuthService.Api.Infrastructure;

public static class ServicesRegistrator
{
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ITokenRepository, RefreshTokenRepository>();
        builder.Services.AddScoped<IUserHistoryRepository, UserHistoryRepository>();

        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<CreateUserCommandHandler>();
            
            cfg.AddOpenBehavior(typeof(LoggingPipelineBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
        });
        
        builder.Services.AddValidatorsFromAssembly(ApplicationAssembly.Assembly, includeInternalTypes: true);
        
        return builder;
    }

    public static WebApplicationBuilder AddCors(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(corsOptions =>
        {
            corsOptions.AddPolicy("frontend",x =>
                x.WithOrigins("https://langskillup.ru","http://82.146.62.254","http://localhost:3000")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
        });

        return builder;
    }

    public static WebApplicationBuilder AddDataLayer(this WebApplicationBuilder builder)
    {
        var dbUser = builder.Configuration["AuthDbConnection:AUTH_DB_USER"];
        var dbPassword = builder.Configuration["AuthDbConnection:AUTH_DB_PASSWORD"];
        var dbHost = builder.Configuration["AuthDbConnection:AUTH_DB_HOST"];
        var db = builder.Configuration["AuthDbConnection:AUTH_DB"];
        var dbPort = builder.Configuration["AuthDbConnection:AUTH_DB_PORT"];
            
        string connectionString = $"User ID={dbUser};password={dbPassword};port={dbPort};host={dbHost};database={db}";
        
        builder.Services.AddDbContext<UserDb>(options =>
        {
            options.UseNpgsql(connectionString, b => 
                    b.MigrationsAssembly("AuthService.Migrations"));
        });

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        using var scope = builder.Services.BuildServiceProvider().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<UserDb>();
        dbContext.Database.Migrate();

        return builder;
    }

    public static WebApplicationBuilder AddSecurity(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        builder.Services.AddScoped<RoleIdentifier>();
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

    public static WebApplicationBuilder AddLoggingWithSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((ctx, config) =>
        {
            config.ReadFrom.Configuration(ctx.Configuration);
        });

        return builder;
    }
    
    public static WebApplicationBuilder AddBackgroundJobs(this WebApplicationBuilder builder)
    {
        builder.Services.AddQuartz(cfg =>
        {
            var key = new JobKey(nameof(OutboxMessageJob));
            var key2 = new JobKey(nameof(DeleteUnactivatedUserJob));
            var key3 = new JobKey(nameof(RefreshTokenCleanerJob));

            cfg.AddJob<OutboxMessageJob>(key)
                .AddTrigger(tg => 
                    tg.ForJob(key)
                        .WithSimpleSchedule(schedule => 
                            schedule.WithIntervalInSeconds(10)
                                .RepeatForever()));
            
            cfg.AddJob<DeleteUnactivatedUserJob>(key2)
                .AddTrigger(tg => 
                    tg.ForJob(key2)
                        .WithSimpleSchedule(schedule => 
                            schedule.WithIntervalInMinutes(1)
                                .RepeatForever()));
            
            cfg.AddJob<RefreshTokenCleanerJob>(key3)
                .AddTrigger(tg => 
                    tg.ForJob(key3)
                        .WithSimpleSchedule(schedule => 
                            schedule.WithIntervalInHours(24)
                                .RepeatForever()));
        });

        builder.Services.AddQuartzHostedService();

        return builder;
    }
}