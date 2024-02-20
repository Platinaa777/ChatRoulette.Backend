using MassTransit;
using MassTransit.Client.Configuration;
using Microsoft.Extensions.Options;

namespace AuthService.Api.Infrastructure;

public static class EventBusExtension
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
}