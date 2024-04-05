using FluentValidation;

namespace ProfileService.Application.Commands.AddDoomSlayerPoint;

public class AddDoomSlayerPointCommandValidator
    : AbstractValidator<AddDoomSlayerPointCommand>
{
    public AddDoomSlayerPointCommandValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
    }
}