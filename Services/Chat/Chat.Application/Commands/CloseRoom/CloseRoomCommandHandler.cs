using Chat.Application.Extensions;
using Chat.Application.Models;
using Chat.Domain.Aggregates;
using Chat.Domain.Aggregates.Room;
using Chat.Domain.Errors;
using Chat.Domain.Repositories;
using DomainDriverDesignAbstractions;
using MassTransit.Client.EventBus;
using MassTransit.Contracts.UserEvents;
using MediatR;

namespace Chat.Application.Commands.CloseRoom;

public class CloseRoomCommandHandler : IRequestHandler<CloseRoomCommand, Result<TwoSeatsRoomInformation>>
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
    
    public async Task<Result<TwoSeatsRoomInformation>> Handle(CloseRoomCommand request, CancellationToken cancellationToken)
    {
        var rooms = await _roomRepository.GetAllRooms();
        TwoSeatsRoom? room = null;
        
        foreach (var dbRoom in rooms)
        {
            if (dbRoom.ContainsPeerConnectionId(request.ConnectionId))
            {
                room = dbRoom;
                break;
            }
        }
        
        if (room is null)
            return Result.Failure<TwoSeatsRoomInformation>(RoomErrors.RoomNotFoundByParticipant);
        
        // delete our room from database
        var closeRoomResponse = await _roomRepository.CloseRoom(room.Id);

        if (!closeRoomResponse)
            return Result.Failure<TwoSeatsRoomInformation>(RoomErrors.RoomCantClose);

        if (room.PeerLinks.Count != 2)
            return Result.Failure<TwoSeatsRoomInformation>(RoomErrors.NotFullRoom);
        
        var chatUser1 = await _chatUserRepository.FindByEmail(room.PeerLinks[0].Email);
        var chatUser2 = await _chatUserRepository.FindByEmail(room.PeerLinks[1].Email);
            
        if (chatUser1 is null || chatUser2 is null)
            return Result.Failure<TwoSeatsRoomInformation>(RoomErrors.NotFullRoom);
            
        int durationOfConversation = room.Close();
            
        chatUser1.AddPeerToHistory(chatUser2);
        chatUser2.AddPeerToHistory(chatUser1);
            
        await _chatUserRepository.Update(chatUser1);
        await _chatUserRepository.Update(chatUser2);

        if (durationOfConversation > 0)
        {
            _busClient.PublishAsync(new UserWasTalked(chatUser1.Email, durationOfConversation),
                cancellationToken).IngoreAsyncWarning();
            _busClient.PublishAsync(new UserWasTalked(chatUser2.Email, durationOfConversation),
                cancellationToken).IngoreAsyncWarning();    
        }

        return new TwoSeatsRoomInformation
        {
            RoomId = room.Id,
            FirstUser = new ChatUserInformation()
            {
                ConnectionId = chatUser1.ConnectionId,
                Email = chatUser1.Email
            },
            SecondUser = new ChatUserInformation()
            {
                ConnectionId = chatUser2.ConnectionId,
                Email = chatUser2.Email
            }
        };
    }
}