using DomainDriverDesignAbstractions;
using MediatR;

namespace ProfileService.Application.Commands.ChangeNickNameProfile;

public class ChangeNickNameProfileCommand : IRequest<Result>
{
    public string NickName { get; set; } = "";
    public string Email { get; set; } = "";
}