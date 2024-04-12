using Chat.Domain.Aggregates;
using Chat.Domain.Aggregates.Room;
using MediatR;

namespace Chat.Application.Queries.GetRoom;

public class GetRoomQuery : IRequest<TwoSeatsRoom?>
{
    public string RoomId { get; set; }    
}