using Chat.Application.Commands;
using Chat.Application.Commands.ConnectUser;
using Chat.HttpModels.HttpRequests;
using Chat.HttpModels.HttpResponses;

namespace Chat.Api.Extensions.Mappers;

public static class RequestMapper
{
    public static ConnectUserCommand ToCommand(this UserJoinRequest request)
        => new ConnectUserCommand()
        {
            Email = request.Email,
            ConnectionId = request.ConnectionId
        };
}