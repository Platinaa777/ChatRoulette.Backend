using AuthService.Application.Constants;
using FluentValidation;

namespace AuthService.Application.Commands.LogoutUser;

public class LogoutUserCommandValidator : AbstractValidator<LogoutUserCommand>
{
    public LogoutUserCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage(ValidationConstants.EmptyRefreshToken);
    }
}