using Chat.HttpModels.HttpResponses;
using MediatR;

namespace Chat.Application.Commands.ConnectUser;

public class ConnectUserCommand : IRequest<UserJoinResponse?>
{
    public string Email { get; set; }
    public string ConnectionId { get; set; }
}
