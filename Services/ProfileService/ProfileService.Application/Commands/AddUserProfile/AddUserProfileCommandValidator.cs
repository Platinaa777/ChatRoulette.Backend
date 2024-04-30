using FluentValidation;
using ProfileService.Application.Constants;

namespace ProfileService.Application.Commands.AddUserProfile;

public class AddUserProfileCommandValidator : AbstractValidator<AddUserProfileCommand>
{
    public AddUserProfileCommandValidator()
    {
        RuleFor(x => x.UserName)
            .MinimumLength(6)
            .WithMessage(ValidationConstants.NotEnoughLettersInUserName);
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage(ValidationConstants.InvalidEmail);
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(ValidationConstants.EmptyIdOfProfile);
    }
}