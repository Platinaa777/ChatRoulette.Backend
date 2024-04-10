using Chat.HttpModels.HttpResponses;
using DomainDriverDesignAbstractions;
using MediatR;

namespace Chat.Application.Commands.ConnectUser;

public class ConnectUserCommand : IRequest<Result<UserJoinResponse>>
{
    public string Email { get; set; } = string.Empty;
    public string ConnectionId { get; set; } = string.Empty;
}
