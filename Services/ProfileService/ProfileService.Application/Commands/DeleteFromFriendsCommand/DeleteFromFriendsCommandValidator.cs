using FluentValidation;

namespace ProfileService.Application.Commands.DeleteFromFriendsCommand;

public class DeleteFromFriendsCommandValidator : AbstractValidator<DeleteFromFriendsCommand>
{
    public DeleteFromFriendsCommandValidator()
    {
        RuleFor(x => x.SenderEmail).EmailAddress();
        RuleFor(x => x.FriendEmail).EmailAddress();
    }
}