using FluentValidation;

namespace ProfileService.Application.Commands.ChangeUserNameProfile;

public class ChangeUserNameProfileCommandValidator : AbstractValidator<ChangeUserNameProfileCommand>
{
    public ChangeUserNameProfileCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.Email).EmailAddress();
    }
}