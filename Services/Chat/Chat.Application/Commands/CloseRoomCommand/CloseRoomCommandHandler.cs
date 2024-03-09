using Chat.Domain.Repositories;
using MediatR;

namespace Chat.Application.Commands.CloseRoomCommand;

public class CloseRoomCommandHandler : IRequestHandler<CloseRoomCommand, bool>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IChatUserRepository _chatUserRepository;

    public CloseRoomCommandHandler(IRoomRepository roomRepository, IChatUserRepository chatUserRepository)
    {
        _roomRepository = roomRepository;
        _chatUserRepository = chatUserRepository;
    }
    
    public async Task<bool> Handle(CloseRoomCommand request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.FindRoomById(request.RoomId);

        if (room is null) return false;
        
        // delete our room from database
        var closeRoomResponse = await _roomRepository.CloseRoom(request.RoomId);

        if (!closeRoomResponse) return false;

        if (room.Peers.Count == 2)
        {
            var chatUser1 = await _chatUserRepository.FindById(room.Peers[0]!.Id);
            var chatUser2 = await _chatUserRepository.FindById(room.Peers[1]!.Id);

            chatUser1.PreviousParticipantIds.Add(chatUser2!.Id);
            chatUser2.PreviousParticipantIds.Add(chatUser1!.Id);

            await _chatUserRepository.Update(chatUser1);
            await _chatUserRepository.Update(chatUser2);
        }

        return true;
    }
}