using AuthService.Application.Constants;
using FluentValidation;

namespace AuthService.Application.Commands.LoginUser;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Password)
            .MinimumLength(4)
            .WithMessage(ValidationConstants.NotEnoughLettersInPassword);
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage(ValidationConstants.InvalidEmail);
    }
}