using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Otlp.Configuration.Extensions;

public static class OtlpExtensions
{
    public static WebApplicationBuilder AddMetricsAndTracing(this WebApplicationBuilder builder)
    {
        Console.WriteLine("OTLP CONNECTION: " + builder.Configuration.GetSection("OtlpExporter:Uri").Value!);
        
        builder.Services.AddOpenTelemetry()
            .WithTracing(b =>
            {
                b.SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                    .AddService(Assembly.GetCallingAssembly()?.GetName().Name ?? "OTLP"));

                b.AddHttpClientInstrumentation();
                b.AddAspNetCoreInstrumentation();
                b.AddEntityFrameworkCoreInstrumentation();
                b.AddRedisInstrumentation();
                b.AddSqlClientInstrumentation();
                b.AddMassTransitInstrumentation();
                b.AddOtlpExporter(opt =>
                {
                    opt.Endpoint = new Uri(builder.Configuration.GetSection("OtlpExporter:Uri").Value!);
                });
            });
        
        return builder;
    }
}