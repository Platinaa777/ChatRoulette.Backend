using FluentValidation;
using ProfileService.Application.Constants;

namespace ProfileService.Application.Commands.ChangeAvatar;

public class ChangeAvatarCommandValidator : AbstractValidator<ChangeAvatarCommand>
{
    public ChangeAvatarCommandValidator()
    {
        RuleFor(x => x.Avatar)
            .NotNull()
            .WithMessage(ValidationConstants.EmptyAvatar);
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage(ValidationConstants.InvalidEmail);
    }
}