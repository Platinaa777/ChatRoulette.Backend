using FluentValidation;

namespace AuthService.Application.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Age).LessThan(100).GreaterThan(10);
        RuleFor(x => x.NickName).NotEmpty();
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}