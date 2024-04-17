using Amazon.S3;
using AuthService.Application.JwtConfig;
using AuthService.Infrastructure.Extensions.Jwt;
using AuthService.Infrastructure.JwtGenerator;
using Chat.GrpcClient;
using Chat.GrpcClient.Configuration;
using DomainDriverDesignAbstractions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using Npgsql;
using ProfileService.Api.BackgroundJobs;
using ProfileService.Api.Utils;
using ProfileService.Application.Assembly;
using ProfileService.Application.Behaviors;
using ProfileService.Application.Queries.GetUserProfile;
using ProfileService.Application.QueryReposAbstractions;
using ProfileService.Domain.Models.FriendInvitationAggregate.Repos;
using ProfileService.Domain.Models.UserHistoryAggregate.Repos;
using ProfileService.Domain.Models.UserProfileAggregate.Repos;
using ProfileService.Infrastructure.Cache;
using ProfileService.Infrastructure.Configuration;
using ProfileService.Infrastructure.PersistenceAbstractions;
using ProfileService.Infrastructure.QueryRepos;
using ProfileService.Infrastructure.Repos.Common;
using ProfileService.Infrastructure.Repos.Implementations.Friend;
using ProfileService.Infrastructure.Repos.Implementations.History;
using ProfileService.Infrastructure.Repos.Implementations.Profile;
using Quartz;
using S3.Client;
using Serilog;

namespace ProfileService.Api.Extensions;

public static class ServicesRegistrator
{
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        
        builder.Services.AddScoped<IUserProfileRepository, CacheUserProfileRepository>();
        builder.Services.AddScoped<UserProfileRepository>();
        
        builder.Services.AddScoped<IFriendInvitationRepository, FriendInvitationRepository>();
        builder.Services.AddScoped<IUserHistoryRepository, UserHistoryRepository>();

        builder.Services.AddScoped<IUserQueryRepository, UserQueryRepository>();
        
        builder.Services.AddSingleton<CredentialsChecker>();
        builder.Services.AddSingleton<JwtTokenCreator>();
        builder.Services.Configure<Jwt>(builder.Configuration.GetSection("Jwt"));
        builder.AddJwtAuthentication(builder.Configuration.GetSection("Jwt:Key").Value!);   
        
        builder.Services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblyContaining<GetUserProfileQueryHandler>());
        
        builder.Services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(LoggingPipelineBehavior<,>));
        
        builder.Services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationPipelineBehavior<,>));
        
        builder.Services.AddValidatorsFromAssembly(ProfileApplication.AppAssembly);

        return builder;
    }

    public static WebApplicationBuilder AddDataLayer(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection("DatabaseOptions"));
        builder.Services.AddScoped<IDbConnectionFactory<NpgsqlConnection>, NpgsqlConnectionFactory>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IChangeTracker, ChangeTracker>();

        return builder;
    }

    public static WebApplicationBuilder AddBackgroundJobs(this WebApplicationBuilder builder)
    {
        builder.Services.AddQuartz(cfg =>
        {
            var key = new JobKey(nameof(OutboxBackgroundJob));
        
            cfg.AddJob<OutboxBackgroundJob>(key)
                .AddTrigger(tg => 
                    tg.ForJob(key)
                        .WithSimpleSchedule(schedule => 
                            schedule.WithIntervalInSeconds(10)
                                .RepeatForever()));
            
        });
        
        builder.Services.AddQuartzHostedService();

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

    public static WebApplicationBuilder AddRedisCache(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        builder.Services.AddStackExchangeRedisCache(setup =>
        {
            setup.Configuration = configuration["Redis:Host"];
        });

        return builder;
    }

    public static WebApplicationBuilder AddS3Storage(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IS3Client, S3Client>();
        var serviceUrl = Environment.GetEnvironmentVariable("AWS_SERVICE_URL");
        var region = Environment.GetEnvironmentVariable("AWS_REGION");
        var accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY");
        var secretKey = Environment.GetEnvironmentVariable("AWS_SECRET_KEY");

        if (builder.Configuration["ENV"] == "Production")
        {
            serviceUrl = builder.Configuration["AWS:AWS_SERVICE_URL"];
            region = builder.Configuration["AWS:AWS_REGION"];
            accessKey = builder.Configuration["AWS:AWS_ACCESS_KEY"];
            secretKey = builder.Configuration["AWS:AWS_SECRET_KEY"];
        }
        
        builder.Services.AddSingleton<IAmazonS3>(x =>
        {
            var cfg = new AmazonS3Config
            {
                ServiceURL = serviceUrl,
                AuthenticationRegion = region,
                ForcePathStyle = true
            };

            return new AmazonS3Client(accessKey, secretKey, cfg);
        });

        return builder;
    }

    public static WebApplicationBuilder AddGrpcChatClient(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<ChatApiConnectionString>(
            builder.Configuration.GetSection("ChatApiConnectionString"));
        builder.Services.AddSingleton<ChatApiConnectionString>(
            sp => sp.GetRequiredService<IOptions<ChatApiConnectionString>>().Value);

        builder.Services.AddScoped<IChatGrpcClient, ChatGrpcClient>();
        builder.Services.AddGrpc();

        return builder;
    }

    public static WebApplicationBuilder AddHttpChatClient(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<ChatApiConnectionString>(
            builder.Configuration.GetSection("ChatApiConnectionString"));
        builder.Services.AddSingleton<ChatApiConnectionString>(
            sp => sp.GetRequiredService<IOptions<ChatApiConnectionString>>().Value);
        
        builder.Services.AddHttpClient("ChatClient", client =>
        {
            client.BaseAddress = new Uri($"http://{builder.Configuration["ChatApiConnectionString:Host"]}:{builder.Configuration["ChatApiConnectionString:Port"]}");
        });

        return builder;
    }

}