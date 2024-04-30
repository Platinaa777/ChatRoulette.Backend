using AuthService.Application.Constants;
using FluentValidation;

namespace AuthService.Application.Commands.BanUser;

public class BanUserCommandValidator : AbstractValidator<BanUserCommand>
{
    public BanUserCommandValidator()
    {
        RuleFor(x => x.ViolatorEmail)
            .EmailAddress()
            .WithMessage(ValidationConstants.InvalidEmail);
        RuleFor(x => x.MinutesToBan)
            .GreaterThan(0)
            .WithMessage(ValidationConstants.BanShouldBeGreaterThanZero);
    }
}