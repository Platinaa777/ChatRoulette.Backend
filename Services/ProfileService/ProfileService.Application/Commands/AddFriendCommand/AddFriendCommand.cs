using MediatR;
using ProfileService.Domain.Shared;

namespace ProfileService.Application.Commands.AddFriendCommand;

public class AddFriendCommand : IRequest<Result>
{
    public string InvitationPublisherEmail { get; set; }
    public string InvitationConsumer { get; set; }
}