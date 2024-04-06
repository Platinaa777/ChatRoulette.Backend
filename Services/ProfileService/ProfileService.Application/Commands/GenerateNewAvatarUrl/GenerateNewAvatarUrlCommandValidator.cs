using FluentValidation;

namespace ProfileService.Application.Commands.GenerateNewAvatarUrl;

public class GenerateNewAvatarUrlCommandValidator
    : AbstractValidator<GenerateNewAvatarUrlCommand>
{
    public GenerateNewAvatarUrlCommandValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
    }
}