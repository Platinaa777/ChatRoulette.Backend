using AuthService.Application.Models;
using MediatR;

namespace AuthService.Application.Commands.LoginUser;

public class LoginUserCommand : IRequest<AuthTokens>
{
    public string Email { get; set; }
    public string Password { get; set; }
}