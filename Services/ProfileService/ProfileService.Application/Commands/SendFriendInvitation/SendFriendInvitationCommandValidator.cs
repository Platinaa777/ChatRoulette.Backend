using FluentValidation;

namespace ProfileService.Application.Commands.SendFriendInvitation;

public class SendFriendInvitationCommandValidator : AbstractValidator<SendFriendInvitation.SendFriendInvitationCommand>
{
    public SendFriendInvitationCommandValidator()
    {
        RuleFor(x => x.InvitationReceiverEmail).EmailAddress();
        RuleFor(x => x.InvitationSenderEmail).EmailAddress();
        RuleFor(x => x.InvitationReceiverEmail).NotEqual(x => x.InvitationSenderEmail);
    }
}