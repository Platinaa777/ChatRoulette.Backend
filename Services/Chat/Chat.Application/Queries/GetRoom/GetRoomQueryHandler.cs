using Chat.Domain.Repositories;
using MediatR;

namespace Chat.Application.Queries.GetRoom;

public class GetRoomQueryHandler : IRequestHandler<GetRoomQuery, List<string>>
{
    private readonly IRoomRepository _roomRepository;

    public GetRoomQueryHandler(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }
    
    public async Task<List<string>> Handle(GetRoomQuery request, CancellationToken cancellationToken)
    {
        var room = await _roomRepository.FindRoomById(request.RoomId);
        if (room == null)
            return null;

        return room.Peers.Select(x => x.ConnectionId).ToList();
    }
}