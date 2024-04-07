using Chat.DataContext.Database;
using MassTransit;
using MassTransit.Client.Configuration;
using Microsoft.EntityFrameworkCore;

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

    public static WebApplicationBuilder AddDataLayer(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ChatDbContext>(options =>
        {
            options.UseNpgsql(
                builder.Configuration.GetConnectionString("PostgresSQL"),
                b => 
                    b.MigrationsAssembly("Chat.DataContext"));
        });
        
        using var scope = builder.Services.BuildServiceProvider().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ChatDbContext>();
        dbContext.Database.Migrate();

        return builder;
    }
}