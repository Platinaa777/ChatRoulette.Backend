using MassTransit;
using MassTransit.Client.Configuration;
using ProfileService.Api.Consumers;

namespace ProfileService.Api.Extensions;

public static class EventBusExtension
{
    public static WebApplicationBuilder AddMassTransit(this WebApplicationBuilder builder)
    {
        builder.Services.AddMassTransit(cfg =>
        {
            cfg.SetKebabCaseEndpointNameFormatter();
            cfg.AddConsumer<ProfileUserRegisterConsumer>();
            cfg.AddConsumer<UserRatingCalculatorConsumer>();
            cfg.AddConsumer<ComplaintApprovedConsumer>();

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