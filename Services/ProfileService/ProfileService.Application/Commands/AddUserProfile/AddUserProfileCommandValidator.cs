using FluentValidation;

namespace ProfileService.Application.Commands.AddUserProfile;

public class AddUserProfileCommandValidator : AbstractValidator<AddUserProfile.AddUserProfileCommand>
{
    public AddUserProfileCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Id).NotEmpty();
    }
}