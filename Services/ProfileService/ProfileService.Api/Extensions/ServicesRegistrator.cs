using DomainDriverDesignAbstractions;
using FluentValidation;
using MediatR;
using Npgsql;
using ProfileService.Api.BackgroundJobs;
using ProfileService.Application.Assembly;
using ProfileService.Application.Behaviors;
using ProfileService.Application.Queries.GetUserProfileQuery;
using ProfileService.Domain.Models.FriendInvitationAggregate.Repos;
using ProfileService.Domain.Models.UserProfileAggregate.Repos;
using ProfileService.Infrastructure.Configuration;
using ProfileService.Infrastructure.Repos.Common;
using ProfileService.Infrastructure.Repos.Implementations.Friend;
using ProfileService.Infrastructure.Repos.Implementations.Profile;
using ProfileService.Infrastructure.Repos.Interfaces;
using Quartz;

namespace ProfileService.Api.Extensions;

public static class ServicesRegistrator
{
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        builder.Services.AddScoped<IFriendInvitationRepository, FriendInvitationRepository>();
        
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
}