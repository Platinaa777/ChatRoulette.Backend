using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Chat.HttpModels.HttpResponses;
using MediatR;

namespace Chat.Application.Commands.ConnectUser;

public class ConnectUserCommandHandler : IRequestHandler<ConnectUserCommand, UserJoinResponse?>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IChatUserRepository _chatUserRepository;
    public ConnectUserCommandHandler(IRoomRepository roomRepository, IChatUserRepository chatUserRepository)
    {
        _roomRepository = roomRepository;
        _chatUserRepository = chatUserRepository;
    }
    
    public async Task<UserJoinResponse?> Handle(ConnectUserCommand request, CancellationToken cancellationToken)
    {
        var chatUser = await _chatUserRepository.FindById(request.ConnectionId!);

        if (chatUser is null)
        {
            chatUser = new ChatUser(request.Email!, request.ConnectionId!);
            // store user in database
            await _chatUserRepository.Add(chatUser);
        }
            
        var room = await _roomRepository.TryToConnectRoom(chatUser);

        // connect user to new created room because not any room exists
        if (room is null)
            room = await _roomRepository.CreateRoomWithConnect(chatUser);
        
        return new UserJoinResponse()
        {
            ConnectionId = request.ConnectionId!,
            Email = request.Email!,
            RoomId = room.Id,
            CreateOffer = room.Peers.Count == 2
        };
    }
}