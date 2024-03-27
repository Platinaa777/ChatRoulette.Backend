namespace Ocelot.Gateway.Middlewares;

public class LoggingRequestMiddleware : IMiddleware
{
    private readonly ILogger<LoggingRequestMiddleware> _logger;

    public LoggingRequestMiddleware(ILogger<LoggingRequestMiddleware> logger)
    {
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        _logger.LogInformation($"Incoming request {context.Request.Path}");

        await next(context);
        
        _logger.LogInformation($"Outcoming request {context.Request.Path}");
    }
}