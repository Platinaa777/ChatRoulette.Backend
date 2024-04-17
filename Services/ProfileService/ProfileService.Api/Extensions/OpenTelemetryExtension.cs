using System.Reflection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace ProfileService.Api.Extensions;

public static class OpenTelemetryExtension
{
    public static WebApplicationBuilder AddOpenTelemetryTracing(this WebApplicationBuilder builder)
    {
        Console.WriteLine("OTLP CONNECTION: " + builder.Configuration.GetSection("OtlpExporter:Uri").Value!);
        Console.WriteLine(Assembly.GetCallingAssembly()?.GetName().Name ?? "OTLP");
        
        builder.Services.AddOpenTelemetry()
            .WithTracing(b =>
            {
                b.SetResourceBuilder(
                        ResourceBuilder.CreateDefault()
                            .AddService("ProfileService"))
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddRedisInstrumentation()
                    .AddSqlClientInstrumentation()
                    .AddMassTransitInstrumentation()
                    .AddOtlpExporter(opt =>
                    {
                        opt.Endpoint = new Uri(builder.Configuration.GetSection("OtlpExporter:Uri").Value!);
                    });
            })
            .ConfigureResource(x => x.Build());
        
        return builder;
        
        // builder.Services.AddOpenTelemetry()
        //     .WithTracing(tracerProviderBuilder =>
        //         tracerProviderBuilder
        //
        //             .AddSource(DiagnosticsConfig.ActivitySource.Name)
        //             .ConfigureResource(resource => resource
        //
        //                 .AddService(DiagnosticsConfig.ServiceName))
        //             .AddHttpClientInstrumentation()
        //             .AddAspNetCoreInstrumentation()
        //             .AddJaegerExporter());
    }
}