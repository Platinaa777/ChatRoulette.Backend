using DomainDriverDesignAbstractions;

namespace AdminService.Application.Validations;

public interface IValidationResult
{
    public static Error DefaultValidationError = new Error(
        "ValidationError", "Validation error was thrown");
    List<Error> Errors { get; set; }
}