using AuthService.Api.Consumers;
using MassTransit;
using MassTransit.Client.Configuration;

namespace AuthService.Api.Infrastructure;

public static class EventBusExtension
{
    public static WebApplicationBuilder AddMassTransit(this WebApplicationBuilder builder)
    {
        builder.Services.AddMassTransit(cfg =>
        {
            cfg.SetKebabCaseEndpointNameFormatter();
            cfg.AddConsumer<UserSubmitEmailConsumer>();
            
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