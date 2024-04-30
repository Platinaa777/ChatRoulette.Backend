using AuthService.Application.Constants;
using FluentValidation;

namespace AuthService.Application.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.UserName)
            .MinimumLength(6)
            .WithMessage(ValidationConstants.NotEnoughLettersInUserName);
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage(ValidationConstants.InvalidEmail);
        RuleFor(x => x.Password)
            .MinimumLength(4)
            .WithMessage(ValidationConstants.NotEnoughLettersInPassword);
    }
}