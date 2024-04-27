using FluentValidation;

namespace ProfileService.Application.Commands.SendFriendInvitation;

public class SendFriendInvitationCommandValidator : AbstractValidator<SendFriendInvitationCommand>
{
    public SendFriendInvitationCommandValidator()
    {
        RuleFor(x => x.InvitationReceiverEmail).EmailAddress();
        RuleFor(x => x.InvitationSenderEmail).EmailAddress();
        RuleFor(x => x.InvitationReceiverEmail)
            .NotEqual(x => x.InvitationSenderEmail)
            .WithMessage("Invitation cant send to yourself");
    }
}