using System.Reflection;
using FluentValidation;
using MediatR;
using Npgsql;
using ProfileService.Application.Assembly;
using ProfileService.Application.Behaviors;
using ProfileService.Application.Queries.GetUserProfileQuery;
using ProfileService.Domain.Models.UserProfileAggregate.Repos;
using ProfileService.Domain.Shared;
using ProfileService.Infrastructure.Configuration;
using ProfileService.Infrastructure.Repos.ConnectionFactories;
using ProfileService.Infrastructure.Repos.Implementations;
using ProfileService.Infrastructure.Repos.Interfaces;
using ProfileService.Infrastructure.Repos.UoW;

namespace ProfileService.Api.Extensions;

public static class ServicesRegistrator
{
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
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
        builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IChangeTracker, ChangeTracker>();

        return builder;
    }
}