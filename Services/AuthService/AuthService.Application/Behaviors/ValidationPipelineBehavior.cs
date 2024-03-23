using AuthService.Domain.Shared;
using FluentValidation;
using MediatR;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace AuthService.Application.Behaviors;

public class ValidationPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    private readonly IValidator<TRequest>? _validator;

    public ValidationPipelineBehavior(IValidator<TRequest>? validator)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validator is null)
            return await next();

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid)
            return await next();

        var errors = validationResult.Errors
            .Select(failure => new Error(
                failure.PropertyName,
                failure.ErrorMessage))
            .ToArray();

        if (!errors.Any())
            return await next();

        return (TResponse)Result.Failure(errors[0]);
    }
}