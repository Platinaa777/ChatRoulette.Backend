using Serilog;

namespace EmailingService.Api.Infrastructure;

public static class ServiceRegistrator
{
    public static WebApplicationBuilder AddLoggingWithSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((ctx, config) =>
        {
            config.ReadFrom.Configuration(ctx.Configuration);
        });
        return builder;
    }

    public static WebApplicationBuilder AddApplicationLayer(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        return builder;
    }
}