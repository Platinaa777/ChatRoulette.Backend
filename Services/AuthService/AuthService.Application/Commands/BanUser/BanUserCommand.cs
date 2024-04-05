using DomainDriverDesignAbstractions;
using MediatR;

namespace AuthService.Application.Commands.BanUser;

public class BanUserCommand : IRequest<Result>
{
    public string ViolatorEmail { get; set; }
    public int MinutesToBan { get; set; }
}