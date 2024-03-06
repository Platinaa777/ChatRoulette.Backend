using MediatR;

namespace Chat.Application.Queries.GetRoom;

public class GetRoomQuery : IRequest<List<string>>
{
    public string RoomId { get; set; }    
}