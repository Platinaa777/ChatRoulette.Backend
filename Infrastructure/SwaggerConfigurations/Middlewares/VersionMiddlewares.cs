using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace SwaggerConfigurations.Middlewares;

public class VersionMiddlewares
{
    private readonly RequestDelegate _next;

    public VersionMiddlewares(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version;
        var serviceName = Assembly.GetExecutingAssembly().GetName().Name;

        var data = new
        {
            version = version,
            serviceName = serviceName
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(data));
    }
}

public static class VersionExtensions
{
    public static IApplicationBuilder UseVersion(this IApplicationBuilder builder)
    {
        return builder.Map("/version", (builder) => builder.UseMiddleware<VersionMiddlewares>());
    }
}
