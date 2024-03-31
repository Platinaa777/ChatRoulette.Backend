using MediatR;
using ProfileService.Domain.Models.FriendInvitationAggregate.Events;
using ProfileService.Domain.Models.UserProfileAggregate;
using ProfileService.Domain.Models.UserProfileAggregate.Repos;
using ProfileService.Domain.Shared;

namespace ProfileService.Application.Events;

public class AcceptedInvitationDomainEventHandler 
    : INotificationHandler<AcceptedInvitationDomainEvent>
{
    private readonly IUserProfileRepository _profileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AcceptedInvitationDomainEventHandler(
        IUserProfileRepository profileRepository,
        IUnitOfWork unitOfWork)
    {
        _profileRepository = profileRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task Handle(AcceptedInvitationDomainEvent notification, CancellationToken cancellationToken)
    {
        await _unitOfWork.StartTransaction(cancellationToken);
        
        var task1 = _profileRepository.FindUserByIdAsync(notification.senderId);
        var task2 = _profileRepository.FindUserByIdAsync(notification.receiverId);

        Task.WaitAll(task1, task2);

        var profileUser1 = await task1;
        var profileUser2 = await task2;

        if (profileUser1 is null || profileUser2 is null)
            return;
        
        profileUser1.AddFriend(profileUser2);
        profileUser2.AddFriend(profileUser1);

        var result1 = _profileRepository.UpdateUserAsync(profileUser1);
        var result2 = _profileRepository.UpdateUserAsync(profileUser2);

        Task.WaitAll(result1, result2);

        if (!(await result1) || !(await result2))
            throw new ArgumentException("Database updating friends list of two users was occured exception");

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}