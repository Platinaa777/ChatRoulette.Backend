using DomainDriverDesignAbstractions;
using MediatR;
using ProfileService.Domain.Models.UserProfileAggregate.Entities;
using ProfileService.Domain.Models.UserProfileAggregate.Events;
using ProfileService.Domain.Models.UserProfileAggregate.Repos;

namespace ProfileService.Application.Events;

public class GotManyFriendsDomainEventHandler
    : INotificationHandler<GotManyFriendsDomainEvent>
{
    private readonly IUserProfileRepository _userProfileRepository;

    public GotManyFriendsDomainEventHandler(
        IUserProfileRepository userProfileRepository)
    {
        _userProfileRepository = userProfileRepository;
    }
    
    public async Task Handle(GotManyFriendsDomainEvent notification, CancellationToken cancellationToken)
    {
        var userProfile = await _userProfileRepository.FindUserByIdAsync(notification.ProfileId);
        if (userProfile is null)
            throw new ArgumentException($"User not found with id: {notification.ProfileId}");

        // load from s3 the reference
        var manyFriendsAchievement = Achievement.Create(2, "many friends");
        if (manyFriendsAchievement.IsFailure)
            throw new ArgumentException("Cant set new achievement");
        
        userProfile.AddAchievement(manyFriendsAchievement.Value);
        await _userProfileRepository.UpdateUserAsync(userProfile);
    }
}