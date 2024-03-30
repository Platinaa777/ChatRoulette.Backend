using MediatR;
using ProfileService.Domain.Shared;

namespace ProfileService.Application.Commands.SendFriendInvitationCommand;

public class SendFriendInvitationCommand : IRequest<Result>
{
    public string InvitationSenderEmail { get; set; }
    public string InvitationReceiverEmail { get; set; }

    public SendFriendInvitationCommand(string invitationSenderEmail, string invitationReceiverEmail)
    {
        InvitationSenderEmail = invitationSenderEmail;
        InvitationReceiverEmail = invitationReceiverEmail;
    }
}