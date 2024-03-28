using FluentValidation;

namespace ProfileService.Application.Commands.ChangeNickNameProfileCommand;

public class ChangeNickNameProfileCommandValidator : AbstractValidator<ChangeNickNameProfileCommand>
{
    public ChangeNickNameProfileCommandValidator()
    {
        RuleFor(x => x.NickName).NotEmpty();
        RuleFor(x => x.Email).EmailAddress();
    }
}