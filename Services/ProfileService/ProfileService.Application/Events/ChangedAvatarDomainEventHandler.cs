using DomainDriverDesignAbstractions;
using MediatR;
using ProfileService.Domain.Models.Identity;
using ProfileService.Domain.Models.UserHistoryAggregate;
using ProfileService.Domain.Models.UserHistoryAggregate.Repos;
using ProfileService.Domain.Models.UserProfileAggregate.Events;

namespace ProfileService.Application.Events;

public class ChangedAvatarDomainEventHandler
    : INotificationHandler<ChangedAvatarDomainEvent>
{
    private readonly IUserHistoryRepository _historyRepository;

    public ChangedAvatarDomainEventHandler(
        IUserHistoryRepository historyRepository)
    {
        _historyRepository = historyRepository;
    }
    
    public async Task Handle(ChangedAvatarDomainEvent notification, CancellationToken cancellationToken)
    {
        var userId = Id.Create(notification.UserId).Value;

        var userHistory = await _historyRepository.FindByUserId(userId);

        if (userHistory is null)
        {
            userHistory = UserHistory.Create(
                id: Guid.NewGuid().ToString(),
                notification.UserId,
                doomSlayerPoints: 0,
                avatarPoints: 0).Value;

            await _historyRepository.AddHistory(userHistory);
        }
        
        userHistory.IncreaseAvatarPoints();

        await _historyRepository.UpdateHistory(userHistory);
    }
}