using Chat.Domain.Aggregates;
using Chat.Domain.Aggregates.Room;
using Chat.Domain.Repositories;
using MediatR;

namespace Chat.Application.Queries.GetAllRooms;

public class GetAllRoomsQueryHandler : IRequestHandler<GetAllRoomsQuery, List<TwoSeatsRoom>>
{
    private readonly IRoomRepository _roomRepository;

    public GetAllRoomsQueryHandler(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }
    
    public async Task<List<TwoSeatsRoom>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
    {
        var response = await _roomRepository.GetAllRooms();

        return response;
    }
}