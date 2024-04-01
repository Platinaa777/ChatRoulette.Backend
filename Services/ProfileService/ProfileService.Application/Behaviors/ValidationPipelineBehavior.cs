using DomainDriverDesignAbstractions;
using FluentValidation;
using MediatR;
using ProfileService.Application.Validations;

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
        if (!_validators.Any())
            return await next();

        Error[] errors = _validators
            .Select(validator => validator.Validate(request))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validation => validation is not null)
            .Select(fail => new Error(
                fail.PropertyName, fail.ErrorMessage))
            .Distinct()
            .ToArray();

        if (errors.Any())
        {
            return ToValidationResult<TResponse>(errors);
        }

        return await next();
    }

    private static TResult ToValidationResult<TResult>(Error[] errors)
        where TResult : Result
    {
        if (typeof(TResult) == typeof(Result))
        {
            return (Validations.ValidationResult.WithErrors(errors) as TResult)!;
        }

        var result = typeof(ValidationResult<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
            .GetMethod(nameof(Validations.ValidationResult.WithErrors))!
            .Invoke(null, new object?[] { errors });

        return (result as TResult)!;
    }
}