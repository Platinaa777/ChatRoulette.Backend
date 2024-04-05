using FluentValidation;

namespace ProfileService.Application.Commands.ChangeNickNameProfile;

public class ChangeNickNameProfileCommandValidator : AbstractValidator<ChangeNickNameProfile.ChangeNickNameProfileCommand>
{
    public ChangeNickNameProfileCommandValidator()
    {
        RuleFor(x => x.NickName).NotEmpty();
        RuleFor(x => x.Email).EmailAddress();
    }
}