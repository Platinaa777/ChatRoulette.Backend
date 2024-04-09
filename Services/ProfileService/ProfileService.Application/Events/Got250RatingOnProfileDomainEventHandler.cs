using DomainDriverDesignAbstractions;
using MediatR;
using ProfileService.Domain.Models.UserProfileAggregate.Entities;
using ProfileService.Domain.Models.UserProfileAggregate.Events;
using ProfileService.Domain.Models.UserProfileAggregate.Repos;

namespace ProfileService.Application.Events;

public class Got250RatingOnProfileDomainEventHandler
    : INotificationHandler<Got250RatingOnProfileDomainEvent>
{
    private readonly IUserProfileRepository _userProfileRepository;

    public Got250RatingOnProfileDomainEventHandler(
        IUserProfileRepository userProfileRepository)
    {
        _userProfileRepository = userProfileRepository;
    }
    
    public async Task Handle(Got250RatingOnProfileDomainEvent notification, CancellationToken cancellationToken)
    {
        var userProfile = await _userProfileRepository.FindUserByIdAsync(notification.ProfileId);
        if (userProfile is null)
            throw new ArgumentException($"Cant find user with {notification.ProfileId}");

        // load from s3 the reference
        var masterOfAdvancementAchievement = Achievement.Create(4, "master");
        if (masterOfAdvancementAchievement.IsFailure)
            throw new ArgumentException("Cant set new achievement");
        
        userProfile.AddAchievement(masterOfAdvancementAchievement.Value);
        await _userProfileRepository.UpdateUserAsync(userProfile);
    }
}