using FluentValidation;

namespace ProfileService.Application.Commands.AddUserProfile;

public class AddUserProfileCommandValidator : AbstractValidator<AddUserProfile.AddUserProfileCommand>
{
    public AddUserProfileCommandValidator()
    {
        RuleFor(x => x.NickName).NotEmpty();
        RuleFor(x => x.Age).GreaterThan(0).LessThan(101);
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Id).NotEmpty();
    }
}