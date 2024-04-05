using FluentValidation;

namespace AuthService.Application.Commands.BanUser;

public class BanUserCommandValidator : AbstractValidator<BanUserCommand>
{
    public BanUserCommandValidator()
    {
        RuleFor(x => x.ViolatorEmail).EmailAddress();
        RuleFor(x => x.MinutesToBan).GreaterThan(0);
    }
}