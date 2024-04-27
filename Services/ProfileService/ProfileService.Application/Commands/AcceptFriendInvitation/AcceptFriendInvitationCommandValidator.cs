using FluentValidation;

namespace ProfileService.Application.Commands.AcceptFriendInvitation;

public class AcceptFriendInvitationCommandValidator : AbstractValidator<AcceptFriendInvitation.AcceptFriendInvitationCommand>
{
    public AcceptFriendInvitationCommandValidator()
    {
        RuleFor(x => x.SenderEmail).EmailAddress();
        RuleFor(x => x.AnswerEmail).EmailAddress();
        RuleFor(x => x.SenderEmail)
            .NotEqual(x => x.AnswerEmail);
    }
}