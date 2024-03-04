using MediatR;

namespace Chat.Application.Queries;

public class GetRoomQuery : IRequest<List<string>>
{
    public string RoomId { get; set; }    
}