using Chat.Domain.Repositories;
using MediatR;

namespace Chat.Application.Commands.CloseRoomCommand;

public class CloseRoomCommandHandler : IRequestHandler<CloseRoomCommand, bool>
{
    private readonly IRoomRepository _roomRepository;

    public CloseRoomCommandHandler(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }
    
    public async Task<bool> Handle(CloseRoomCommand request, CancellationToken cancellationToken)
    {
        return await _roomRepository.CloseRoom(request.RoomId);
    }
}