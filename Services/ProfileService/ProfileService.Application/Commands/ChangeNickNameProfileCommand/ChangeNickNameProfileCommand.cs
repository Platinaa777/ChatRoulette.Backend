using DomainDriverDesignAbstractions;
using MediatR;

namespace ProfileService.Application.Commands.ChangeNickNameProfileCommand;

public class ChangeNickNameProfileCommand : IRequest<Result>
{
    public string NickName { get; set; }
    public string Email { get; set; }
}