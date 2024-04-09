using DomainDriverDesignAbstractions;
using MediatR;
using ProfileService.Domain.Models.FriendInvitationAggregate.Events;
using ProfileService.Domain.Models.UserProfileAggregate.Repos;

namespace ProfileService.Application.Events;

public class AcceptedInvitationDomainEventHandler 
    : INotificationHandler<AcceptedInvitationDomainEvent>
{
    private readonly IUserProfileRepository _profileRepository;

    public AcceptedInvitationDomainEventHandler(
        IUserProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }
    
    public async Task Handle(AcceptedInvitationDomainEvent notification, CancellationToken cancellationToken)
    {
        var profileUser1 = await _profileRepository.FindUserByIdAsync(notification.SenderId);
        var profileUser2 = await _profileRepository.FindUserByIdAsync(notification.ReceiverId);

        if (profileUser1 is null)
            throw new ArgumentException($"User not found with id: {notification.SenderId}");

        if (profileUser2 is null)
            throw new ArgumentException($"User not found with id: {notification.ReceiverId}");
        
        profileUser1.AddFriend(profileUser2);
        profileUser2.AddFriend(profileUser1);

        var result1 = await _profileRepository.UpdateUserAsync(profileUser1);
        var result2 = await _profileRepository.UpdateUserAsync(profileUser2);

        if (!result1 || !result2)
            throw new ArgumentException("Database updating friends list of two users was occured exception");
    }
}