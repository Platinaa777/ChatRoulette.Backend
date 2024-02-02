using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace SwaggerConfigurations.Middlewares;

public class HttpPathMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<HttpPathMiddleware> _logger;

    public HttpPathMiddleware(RequestDelegate next, ILogger<HttpPathMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation(context.Request.Path);
        await _next(context);
    }
}

public static class HttpPathExtensions
{
    public static IApplicationBuilder UseHttpPathInfo(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<HttpPathMiddleware>();
    }
}
