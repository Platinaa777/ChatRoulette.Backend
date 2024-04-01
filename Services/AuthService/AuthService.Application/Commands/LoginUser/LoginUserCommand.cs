using AuthService.Application.Models;
using DomainDriverDesignAbstractions;
using MediatR;

namespace AuthService.Application.Commands.LoginUser;

public class LoginUserCommand : IRequest<Result<AuthTokens>>
{
    public string Email { get; set; }
    public string Password { get; set; }
}