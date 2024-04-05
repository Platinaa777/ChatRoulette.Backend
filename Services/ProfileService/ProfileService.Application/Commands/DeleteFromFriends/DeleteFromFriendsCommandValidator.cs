using FluentValidation;

namespace ProfileService.Application.Commands.DeleteFromFriends;

public class DeleteFromFriendsCommandValidator : AbstractValidator<DeleteFromFriends.DeleteFromFriendsCommand>
{
    public DeleteFromFriendsCommandValidator()
    {
        RuleFor(x => x.SenderEmail).EmailAddress();
        RuleFor(x => x.FriendEmail).EmailAddress();
    }
}