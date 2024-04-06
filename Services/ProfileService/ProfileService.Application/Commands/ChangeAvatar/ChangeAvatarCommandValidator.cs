using FluentValidation;

namespace ProfileService.Application.Commands.ChangeAvatar;

public class ChangeAvatarCommandValidator : AbstractValidator<ChangeAvatarCommand>
{
    public ChangeAvatarCommandValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.ContentType).NotEmpty();
    }
}