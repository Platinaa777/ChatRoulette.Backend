using FluentValidation;

namespace ProfileService.Application.Commands.RejectFriendInvitationCommand;

public class RejectFriendInvitationCommandValidator : AbstractValidator<RejectFriendInvitationCommand>
{
    public RejectFriendInvitationCommandValidator()
    {
        RuleFor(x => x.InvitationReceiverEmail).EmailAddress();
        RuleFor(x => x.InvitationSenderEmail).EmailAddress();
        RuleFor(x => x.InvitationReceiverEmail)
            .NotEqual(x => x.InvitationSenderEmail);
    }
}