using FluentValidation;

namespace ProfileService.Application.Commands.SendFriendInvitationCommand;

public class SendFriendInvitationCommandValidator : AbstractValidator<SendFriendInvitationCommand>
{
    public SendFriendInvitationCommandValidator()
    {
        RuleFor(x => x.InvitationReceiverEmail).NotEmpty();
        RuleFor(x => x.InvitationSenderEmail).NotEmpty();
        RuleFor(x => x.InvitationReceiverEmail).NotEqual(x => x.InvitationSenderEmail);
    }
}