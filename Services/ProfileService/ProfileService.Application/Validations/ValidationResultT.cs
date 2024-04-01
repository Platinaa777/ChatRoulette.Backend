using DomainDriverDesignAbstractions;

namespace ProfileService.Application.Validations;

public class ValidationResult<T> : Result<T>, IValidationResult
{
    protected internal ValidationResult(Error[] errors)
        : base(default, false, IValidationResult.DefaultValidationError)
    {
        Errors = errors.ToList();
    }

    public List<Error> Errors { get; set; }

    public static ValidationResult<T> WithErrors(Error[] errors) => new(errors);
}