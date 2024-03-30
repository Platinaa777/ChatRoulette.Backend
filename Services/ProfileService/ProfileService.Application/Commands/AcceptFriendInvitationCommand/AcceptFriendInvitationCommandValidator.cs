using FluentValidation;

namespace ProfileService.Application.Commands.AcceptFriendInvitationCommand;

public class AcceptFriendInvitationCommandValidator : AbstractValidator<AcceptFriendInvitationCommand>
{
    public AcceptFriendInvitationCommandValidator()
    {
        RuleFor(x => x.InvitationReceiverEmail).EmailAddress();
        RuleFor(x => x.InvitationSenderEmail).EmailAddress();
        RuleFor(x => x.InvitationReceiverEmail)
            .NotEqual(x => x.InvitationSenderEmail);
    }
}