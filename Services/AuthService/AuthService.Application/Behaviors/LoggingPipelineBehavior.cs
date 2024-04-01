using DomainDriverDesignAbstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuthService.Application.Behaviors;

public class LoggingPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;

    public LoggingPipelineBehavior(ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }
    
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting request {@RequestName} {@DateTimeUtc}", 
            typeof(TRequest),
            DateTime.UtcNow);
        
        var result = await next();

        if (result.IsFailure)
        {
            _logger.LogInformation("Error request {@RequestName}, {@Error}, {@DateTimeUtc}", 
                typeof(TRequest),
                result.Error,
                DateTime.UtcNow);

            return result;
        }
        
        _logger.LogInformation("Completed request {@RequestName} {@DateTimeUtc}", 
            typeof(TRequest),
            DateTime.UtcNow);

        return result;
    }
}