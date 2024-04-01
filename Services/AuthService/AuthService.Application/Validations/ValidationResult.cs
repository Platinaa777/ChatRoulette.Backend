using DomainDriverDesignAbstractions;

namespace AuthService.Application.Validations;

public class ValidationResult : Result, IValidationResult
{
    protected ValidationResult(Error[] errors)
        : base(false, IValidationResult.DefaultValidationError)
    {
        Errors = errors.ToList();
    }

    public List<Error> Errors { get; set; }
    public static ValidationResult WithErrors(Error[] errors) => new(errors);
}