using DomainDriverDesignAbstractions;
using MediatR;

namespace AuthService.Application.Commands.LogoutUser;

public class LogoutUserCommand : IRequest<Result>
{
    public string RefreshToken { get; set; }
}