using MediatR;
using ProfileService.Domain.Shared;

namespace ProfileService.Application.Commands.RejectFriendInvitationCommand;

public class RejectFriendInvitationCommand : IRequest<Result>
{
    public string InvitationSenderEmail { get; set; }
    public string InvitationReceiverEmail { get; set; }

    public RejectFriendInvitationCommand(string invitationSenderEmail, string invitationReceiverEmail)
    {
        InvitationSenderEmail = invitationSenderEmail;
        InvitationReceiverEmail = invitationReceiverEmail;
    }
}