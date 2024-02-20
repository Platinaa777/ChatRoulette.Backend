using MassTransit.Client.Configuration;
using MassTransit.Client.EventBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MassTransit.Client.Extensions;


public static class EventBusExtensions
{
    public static WebApplicationBuilder AddEventBusClient(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        
        builder.Services.AddScoped<IEventBusClient, EventBusClient>();
        builder.Services.Configure<EventBusOptions>(configuration.GetSection("MessageBroker"));
        builder.Services.AddSingleton<EventBusOptions>(opt =>
            opt.GetRequiredService<IOptions<EventBusOptions>>().Value);
        
        return builder;
    }    
}