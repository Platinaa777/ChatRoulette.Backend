using FluentValidation;
using ProfileService.Application.Constants;

namespace ProfileService.Application.Commands.DeleteFromFriends;

public class DeleteFromFriendsCommandValidator : AbstractValidator<DeleteFromFriendsCommand>
{
    public DeleteFromFriendsCommandValidator()
    {
        RuleFor(x => x.SenderEmail)
            .EmailAddress()
            .WithMessage(ValidationConstants.SenderEmailInvalid);
        RuleFor(x => x.FriendEmail)
            .EmailAddress()
            .WithMessage(ValidationConstants.ReceiverEmailInvalid);
    }
}