using DomainDriverDesignAbstractions;
using MediatR;
using ProfileService.Domain.Models.UserHistoryAggregate.Events;
using ProfileService.Domain.Models.UserProfileAggregate.Entities;
using ProfileService.Domain.Models.UserProfileAggregate.Repos;

namespace ProfileService.Application.Events;

public class ReceivedAvatarAchievementDomainEventHandler
    : INotificationHandler<ReceivedAvatarAchievementDomainEvent>
{
    private readonly IUserProfileRepository _profileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReceivedAvatarAchievementDomainEventHandler(
        IUserProfileRepository profileRepository,
        IUnitOfWork unitOfWork)
    {
        _profileRepository = profileRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(ReceivedAvatarAchievementDomainEvent notification, CancellationToken cancellationToken)
    {
        await _unitOfWork.StartTransaction(cancellationToken);
        
        var userProfile = await _profileRepository.FindUserByIdAsync(notification.UserId);

        if (userProfile is null)
            throw new ArgumentException($"User not found with id {notification.UserId}");
        
        // instead of photo should insert url to avatar in s3
        var changeAvatarAchievement = Achievement.Create(1, "change avatar");
        if (changeAvatarAchievement.IsFailure)
            throw new ArgumentException("Cant set new achievement");
        
        userProfile.AddAchievement(changeAvatarAchievement.Value);
        await _profileRepository.UpdateUserAsync(userProfile);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}