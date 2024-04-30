using FluentValidation;
using ProfileService.Application.Constants;

namespace ProfileService.Application.Commands.ChangeUserNameProfile;

public class ChangeUserNameProfileCommandValidator : AbstractValidator<ChangeUserNameProfileCommand>
{
    public ChangeUserNameProfileCommandValidator()
    {
        RuleFor(x => x.UserName)
            .MinimumLength(6)
            .WithMessage(ValidationConstants.NotEnoughLettersInUserName);
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage(ValidationConstants.InvalidEmail);
    }
}