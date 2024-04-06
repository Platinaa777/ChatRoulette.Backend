using DomainDriverDesignAbstractions;
using MediatR;
using ProfileService.Domain.Models.Identity;
using ProfileService.Domain.Models.UserHistoryAggregate;
using ProfileService.Domain.Models.UserHistoryAggregate.Repos;
using ProfileService.Domain.Models.UserProfileAggregate.Events;

namespace ProfileService.Application.Events;

public class ChangeAvatarDomainEventHandler
    : INotificationHandler<ChangeAvatarDomainEvent>
{
    private readonly IUserHistoryRepository _historyRepository;
    private readonly IUnitOfWork _unitOfWork;


    public ChangeAvatarDomainEventHandler(
        IUserHistoryRepository historyRepository,
        IUnitOfWork unitOfWork)
    {
        _historyRepository = historyRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(ChangeAvatarDomainEvent notification, CancellationToken cancellationToken)
    {
        await _unitOfWork.StartTransaction(cancellationToken);

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

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}