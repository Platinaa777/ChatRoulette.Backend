using FluentValidation;
using ProfileService.Application.Constants;

namespace ProfileService.Application.Commands.SendFriendInvitation;

public class SendFriendInvitationCommandValidator : AbstractValidator<SendFriendInvitationCommand>
{
    public SendFriendInvitationCommandValidator()
    {
        RuleFor(x => x.InvitationReceiverEmail)
            .EmailAddress()
            .WithMessage(ValidationConstants.ReceiverEmailInvalid);
        RuleFor(x => x.InvitationSenderEmail)
            .EmailAddress()
            .WithMessage(ValidationConstants.SenderEmailInvalid);
        RuleFor(x => x.InvitationReceiverEmail)
            .NotEqual(x => x.InvitationSenderEmail)
            .WithMessage(ValidationConstants.EqualitySenderAndReceiver);
    }
}