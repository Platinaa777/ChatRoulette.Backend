using MassTransit.Contracts.UserEvents;
using MediatR;

namespace AuthService.Application.Commands;

public class ConfirmEmailCommand : IRequest<string>
{
    public ConfirmEmailCommand(string email)
    {
        Email = email;
    }
    public string Email { get; set; }
}

public static class ConfirmEmailCommandExtension
{
    public static ConfirmEmailCommand ToCommand(this UserSubmittedEmail req) =>
        new ConfirmEmailCommand(req.Email);
}