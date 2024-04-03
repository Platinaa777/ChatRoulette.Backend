using Chat.Domain.Entities;
using MediatR;

namespace Chat.Application.Queries.GetRoom;

public class GetRoomQuery : IRequest<TwoSeatsRoom?>
{
    public string RoomId { get; set; }    
}