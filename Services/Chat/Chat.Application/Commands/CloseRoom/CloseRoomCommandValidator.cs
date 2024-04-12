using FluentValidation;

namespace Chat.Application.Commands.CloseRoom;

public class CloseRoomCommandValidator : AbstractValidator<CloseRoomCommand>
{
    public CloseRoomCommandValidator()
    {
        RuleFor(x => x.ConnectionId).NotEmpty();
    }
}