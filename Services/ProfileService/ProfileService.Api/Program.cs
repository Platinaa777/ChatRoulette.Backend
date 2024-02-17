using Npgsql;
using ProfileService.Application.Handlers;
using ProfileService.Domain.Models.UserProfileAggregate.Repos;
using ProfileService.Infrastructure.Configuration;
using ProfileService.Infrastructure.Repos.ConnectionFactories;
using ProfileService.Infrastructure.Repos.Implementations;
using ProfileService.Infrastructure.Repos.Interfaces;
using SwaggerConfigurations.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection("DatabaseOptions"));
builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddScoped<IDbConnectionFactory<NpgsqlConnection>, NpgsqlConnectionFactory>();
builder.AddSwagger();
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<GetUserProfileQueryHandler>());

var app = builder.Build();

app.MapControllers();

app.Run();