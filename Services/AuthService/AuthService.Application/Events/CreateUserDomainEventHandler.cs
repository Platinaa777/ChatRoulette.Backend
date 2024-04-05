using AuthService.Domain.Models.UserAggregate.Events;
using AuthService.Domain.Models.UserHistoryAggregate;
using AuthService.Domain.Models.UserHistoryAggregate.Repos;
using DomainDriverDesignAbstractions;
using MediatR;

namespace AuthService.Application.Events;

public class CreateUserDomainEventHandler
    : INotificationHandler<CreateUserDomainEvent>
{
    private readonly IUserHistoryRepository _userHistoryRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserDomainEventHandler(
        IUserHistoryRepository userHistoryRepository,
        IUnitOfWork unitOfWork)
    {
        _userHistoryRepository = userHistoryRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(CreateUserDomainEvent notification, CancellationToken cancellationToken)
    {
        var history = History.Create(
            historyId: Guid.NewGuid().ToString(),
            notification.UserId).Value;

        await _userHistoryRepository.AddHistory(history);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}