using FluentValidation;

namespace ProfileService.Application.Commands.RejectFriendInvitation;

public class RejectFriendInvitationCommandValidator : AbstractValidator<RejectFriendInvitation.RejectFriendInvitationCommand>
{
    public RejectFriendInvitationCommandValidator()
    {
        RuleFor(x => x.SenderEmail).EmailAddress();
        RuleFor(x => x.AnswerEmail).EmailAddress();
        RuleFor(x => x.SenderEmail)
            .NotEqual(x => x.AnswerEmail);
    }
}