using FluentValidation;

namespace ProfileService.Application.Commands.AddFriendCommand;

public class AddFriendCommandValidator : AbstractValidator<AddFriendCommand>
{
    public AddFriendCommandValidator()
    {
        RuleFor(x => x.InvitationConsumer).NotEmpty();
        RuleFor(x => x.InvitationPublisherEmail).NotEmpty();
    }
}