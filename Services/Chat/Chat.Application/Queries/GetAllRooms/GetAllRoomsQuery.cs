using Chat.Domain.Aggregates;
using Chat.Domain.Aggregates.Room;
using MediatR;

namespace Chat.Application.Queries.GetAllRooms;

public class GetAllRoomsQuery : IRequest<List<TwoSeatsRoom>>
{
    
}