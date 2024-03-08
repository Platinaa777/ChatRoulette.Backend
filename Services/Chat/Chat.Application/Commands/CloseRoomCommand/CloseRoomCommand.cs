using MediatR;

namespace Chat.Application.Commands.CloseRoomCommand;

public class CloseRoomCommand : IRequest<bool>
{
    public string RoomId { get; set; }
}