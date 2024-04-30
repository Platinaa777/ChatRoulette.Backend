using AuthService.Application.Constants;
using FluentValidation;

namespace AuthService.Application.Commands.ConfirmEmail;

public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailCommandValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage(ValidationConstants.InvalidEmail);
    }
}