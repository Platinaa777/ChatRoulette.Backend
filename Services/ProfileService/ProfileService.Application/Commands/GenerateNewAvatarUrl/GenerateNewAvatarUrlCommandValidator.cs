using FluentValidation;
using ProfileService.Application.Constants;

namespace ProfileService.Application.Commands.GenerateNewAvatarUrl;

public class GenerateNewAvatarUrlCommandValidator
    : AbstractValidator<GenerateNewAvatarUrlCommand>
{
    public GenerateNewAvatarUrlCommandValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage(ValidationConstants.InvalidEmail);
    }
}