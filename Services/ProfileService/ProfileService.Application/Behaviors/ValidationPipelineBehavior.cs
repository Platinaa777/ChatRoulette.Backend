using FluentValidation;
using MediatR;
using ProfileService.Domain.Shared;

namespace ProfileService.Application.Behaviors;

public class ValidationPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var failures = _validators
            .Select(v => v.Validate(request))
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        if (failures.Any())
        {
            var errors = failures
                .Select(failure => new Error(
                    failure.PropertyName,
                    failure.ErrorMessage))
                .ToArray();
            
            return (TResponse)Result.Failure(errors[0]);    
        }
        
        return await next();
    }
}