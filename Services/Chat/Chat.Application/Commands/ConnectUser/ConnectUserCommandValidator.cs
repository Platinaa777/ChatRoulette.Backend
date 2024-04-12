using FluentValidation;
using MassTransit;

namespace Chat.Application.Commands.ConnectUser;

public class ConnectUserCommandValidator : AbstractValidator<ConnectUserCommand>
{
    public ConnectUserCommandValidator()
    {
        RuleFor(x => x.ConnectionId).NotNull().NotEmpty();
        RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress();
    }
}