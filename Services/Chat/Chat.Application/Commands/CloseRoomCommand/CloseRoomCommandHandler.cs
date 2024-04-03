using Chat.Domain.Repositories;
using MassTransit.Client.EventBus;
using MassTransit.Contracts.UserEvents;
using MediatR;

namespace Chat.Application.Commands.CloseRoomCommand;

public class CloseRoomCommandHandler : IRequestHandler<CloseRoomCommand, bool>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IChatUserRepository _chatUserRepository;
    private readonly IEventBusClient _busClient;

    public CloseRoomCommandHandler(
        IRoomRepository roomRepository,
        IChatUserRepository chatUserRepository,
        IEventBusClient busClient)
    {
        _roomRepository = roomRepository;
        _chatUserRepository = chatUserRepository;
        _busClient = busClient;
    }
    
    public async Task<bool> Handle(CloseRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.FindRoomById(request.RoomId);

        if (room is null)
            return false;
        
        // delete our room from database
        var closeRoomResponse = await _roomRepository.CloseRoom(request.RoomId);

        if (!closeRoomResponse)
            return false;

        if (room.PeerEmails.Count == 2)
        {
            var chatUser1 = await _chatUserRepository.FindByEmail(room.PeerEmails[0]);
            var chatUser2 = await _chatUserRepository.FindByEmail(room.PeerEmails[1]);
            
            if (chatUser1 is null || chatUser2 is null)
                return true;
            
            int durationOfConversation = room.Close();
            
            chatUser1.AddPeerToHistory(chatUser2);
            chatUser2.AddPeerToHistory(chatUser1);
            
            await _chatUserRepository.Update(chatUser1);
            await _chatUserRepository.Update(chatUser2);
            
            _busClient.PublishAsync(new UserWasTalked(chatUser1.Email, durationOfConversation),
                cancellationToken);
            _busClient.PublishAsync(new UserWasTalked(chatUser2.Email, durationOfConversation),
                cancellationToken);
        }

        return true;
    }
}