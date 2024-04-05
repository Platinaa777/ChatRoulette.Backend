using DomainDriverDesignAbstractions;
using MediatR;

namespace ProfileService.Application.Commands.RejectFriendInvitation;

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