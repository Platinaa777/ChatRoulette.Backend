using FluentValidation;
using ProfileService.Application.Constants;

namespace ProfileService.Application.Commands.RejectFriendInvitation;

public class RejectFriendInvitationCommandValidator : AbstractValidator<RejectFriendInvitation.RejectFriendInvitationCommand>
{
    public RejectFriendInvitationCommandValidator()
    {
        RuleFor(x => x.SenderEmail)
            .EmailAddress()
            .WithMessage(ValidationConstants.SenderEmailInvalid);
        RuleFor(x => x.AnswerEmail)
            .EmailAddress()
            .WithMessage(ValidationConstants.ReceiverEmailInvalid);
        RuleFor(x => x.SenderEmail)
            .NotEqual(x => x.AnswerEmail)
            .WithMessage(ValidationConstants.EqualitySenderAndReceiver);
    }
}