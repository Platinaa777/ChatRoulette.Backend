using DomainDriverDesignAbstractions;
using MediatR;

namespace ProfileService.Application.Commands.AcceptFriendInvitationCommand;

public class AcceptFriendInvitationCommand : IRequest<Result>
{
    public string InvitationSenderEmail { get; set; }
    public string InvitationReceiverEmail { get; set; }

    public AcceptFriendInvitationCommand(string invitationSenderEmail, string invitationReceiverEmail)
    {
        InvitationSenderEmail = invitationSenderEmail;
        InvitationReceiverEmail = invitationReceiverEmail;
    }
}