using FluentValidation;

namespace ProfileService.Application.Commands.AddUserProfileCommand;

public class AddUserProfileCommandValidator : AbstractValidator<AddUserProfileCommand>
{
    public AddUserProfileCommandValidator()
    {
        RuleFor(x => x.NickName).NotEmpty();
        RuleFor(x => x.Age).GreaterThan(0).LessThan(101);
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Id).NotEmpty();
    }
}