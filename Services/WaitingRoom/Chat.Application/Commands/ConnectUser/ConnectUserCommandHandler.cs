using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Chat.HttpModels.HttpResponses;
using MediatR;

namespace Chat.Application.Commands.ConnectUser;

public class ConnectUserCommandHandler : IRequestHandler<ConnectUserCommand, UserJoinResponse?>
{
    private readonly IRoomRepository _roomRepository;

    public ConnectUserCommandHandler(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }
    
    public async Task<UserJoinResponse?> Handle(ConnectUserCommand request, CancellationToken cancellationToken)
    {
        var chatUser = new ChatUser(request.Email, request.ConnectionId);

        var room = await _roomRepository.TryToConnectRoom(chatUser);

        // connect user to new created room because not any room exists
        if (room == null)
            room = await _roomRepository.CreateRoomWithConnect(chatUser);
        
        return new UserJoinResponse()
        {
            // delete this code later
            // todo
            ConnectionId = request.ConnectionId,
            Email = request.Email,
            RoomId = room.Id,
            CreateOffer = room.peers.Count == 2
        };
    }
}