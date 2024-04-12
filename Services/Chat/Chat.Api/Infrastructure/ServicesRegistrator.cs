using Chat.Api.WebSockets;
using Chat.Application.Behavior;
using Chat.Application.Commands.CloseRoom;
using Chat.DataContext.Database;
using Chat.Domain.Repositories;
using Chat.Infrastructure.Repositories;
using FluentValidation;
using Hangfire;
using MassTransit;
using MassTransit.Client.Configuration;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Chat.Api.Infrastructure;

public static class ServicesRegistrator
{
    public static WebApplicationBuilder AddMassTransit(this WebApplicationBuilder builder)
    {
        builder.Services.AddMassTransit(cfg =>
        {
            cfg.SetKebabCaseEndpointNameFormatter();
            
            cfg.UsingRabbitMq((context, cfg) =>
            {
                EventBusOptions options = context.GetRequiredService<EventBusOptions>();

                cfg.Host(new Uri(options.Host), userSettings => 
                {
                    userSettings.Username(options.UserName);
                    userSettings.Password(options.Password);
                });
                
                cfg.ConfigureEndpoints(context);
            });
        });

        return builder;
    }
    
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddScoped<IRoomRepository, RoomRepository>();
        builder.Services.AddScoped<IChatUserRepository, ChatUserRepository>();
        builder.Services.AddScoped<IGameRepository, GameRepository>();
        builder.Services.AddScoped<IRoundRepository, RoundRepository>();

        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<CloseRoomCommandHandler>();
            
            cfg.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
        });
        
        builder.Services.AddValidatorsFromAssembly(
            typeof(CloseRoomCommand).Assembly,
            includeInternalTypes: true);
        
        return builder;
    }

    public static WebApplicationBuilder AddChatMessaging(this WebApplicationBuilder builder)
    {
        builder.Services.AddTransient<ChatHub>();
        builder.Services.AddSignalR(options =>
        {
            options.ClientTimeoutInterval = TimeSpan.MaxValue;
        });

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

    public static WebApplicationBuilder AddDataLayer(this WebApplicationBuilder builder)
    {
        var dbUser = builder.Configuration["ChatDbConnection:CHAT_DB_USER"];
        var dbPassword = builder.Configuration["ChatDbConnection:CHAT_DB_PASSWORD"];
        var dbHost = builder.Configuration["ChatDbConnection:CHAT_DB_HOST"];
        var db = builder.Configuration["ChatDbConnection:CHAT_DB"];
        var dbPort = builder.Configuration["ChatDbConnection:CHAT_DB_PORT"];
        string connectionString = $"User ID={dbUser};password={dbPassword};port={dbPort};host={dbHost};database={db}";
        
        builder.Services.AddDbContext<ChatDbContext>(options =>
        {
            options.UseNpgsql(connectionString, b => 
                    b.MigrationsAssembly("Chat.DataContext"));
        });
        
        using var scope = builder.Services.BuildServiceProvider().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ChatDbContext>();
        dbContext.Database.Migrate();

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

    public static WebApplicationBuilder AddScheduleJob(this WebApplicationBuilder builder)
    {
        builder.Services.AddHangfire((provider, configuration) => { });
        builder.Services.AddHangfireServer();
        return builder;
    }
}