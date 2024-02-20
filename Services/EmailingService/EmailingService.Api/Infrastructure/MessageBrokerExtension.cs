using EmailingService.Api.Consumers;
using MassTransit;
using MassTransit.Client.Configuration;
using Microsoft.Extensions.Options;

namespace EmailingService.Api.Infrastructure;

public static class MessageBrokerExtension
{
    public static WebApplicationBuilder AddMassTransit(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        builder.Services.Configure<EventBusOptions>(configuration.GetSection("MessageBroker"));
        builder.Services.AddSingleton<EventBusOptions>(sp 
            => sp.GetRequiredService<IOptions<EventBusOptions>>().Value);
        
        builder.Services.AddMassTransit(configure =>
        {
            configure.SetKebabCaseEndpointNameFormatter();

            configure.AddConsumer<RegisterUserConsumer>();
            
            configure.UsingRabbitMq((context, cfg) =>
            {
                var rabbitMqOptions = context.GetRequiredService<EventBusOptions>();
                
                cfg.Host(new Uri(rabbitMqOptions.Host), h =>
                {
                    h.Username(rabbitMqOptions.UserName);
                    h.Password(rabbitMqOptions.Password);
                });
                
                cfg.ConfigureEndpoints(context);
            });
        });
        

        return builder;
    }
}