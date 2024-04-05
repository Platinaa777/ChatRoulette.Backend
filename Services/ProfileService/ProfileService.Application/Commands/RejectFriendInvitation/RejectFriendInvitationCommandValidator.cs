using FluentValidation;

namespace ProfileService.Application.Commands.RejectFriendInvitation;

public class RejectFriendInvitationCommandValidator : AbstractValidator<RejectFriendInvitation.RejectFriendInvitationCommand>
{
    public RejectFriendInvitationCommandValidator()
    {
        RuleFor(x => x.InvitationReceiverEmail).EmailAddress();
        RuleFor(x => x.InvitationSenderEmail).EmailAddress();
        RuleFor(x => x.InvitationReceiverEmail)
            .NotEqual(x => x.InvitationSenderEmail);
    }
}