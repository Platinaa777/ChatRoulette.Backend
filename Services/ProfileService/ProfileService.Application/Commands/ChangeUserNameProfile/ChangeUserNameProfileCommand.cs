using DomainDriverDesignAbstractions;
using MediatR;

namespace ProfileService.Application.Commands.ChangeUserNameProfile;

public class ChangeUserNameProfileCommand : IRequest<Result>
{
    public string UserName { get; set; } = "";
    public string Email { get; set; } = "";
}