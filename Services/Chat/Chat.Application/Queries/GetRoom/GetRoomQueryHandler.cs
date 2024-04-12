using Chat.Domain.Aggregates;
using Chat.Domain.Aggregates.Room;
using Chat.Domain.Repositories;
using MediatR;

namespace Chat.Application.Queries.GetRoom;

public class GetRoomQueryHandler : IRequestHandler<GetRoomQuery, TwoSeatsRoom?>
{
    private readonly IRoomRepository _roomRepository;

    public GetRoomQueryHandler(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }
    
    public async Task<TwoSeatsRoom?> Handle(GetRoomQuery request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.FindRoomById(request.RoomId);
        if (room is null)
            return null;

        return room;
    }
}