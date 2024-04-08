using Chat.Application.Models;
using DomainDriverDesignAbstractions;
using MediatR;

namespace Chat.Application.Commands.CloseRoom;

public class CloseRoomCommand : IRequest<Result<TwoSeatsRoomInformation>>
{
    public string ConnectionId { get; set; } = "";
}