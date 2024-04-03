using DomainDriverDesignAbstractions;
using MediatR;
using ProfileService.Domain.Models.FriendInvitationAggregate.Events;
using ProfileService.Domain.Models.UserProfileAggregate.Repos;

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
        
        var profileUser1 = await _profileRepository.FindUserByIdAsync(notification.SenderId);
        var profileUser2 = await _profileRepository.FindUserByIdAsync(notification.ReceiverId);

        if (profileUser1 is null || profileUser2 is null)
            return;
        
        profileUser1.AddFriend(profileUser2);
        profileUser2.AddFriend(profileUser1);

        var result1 = await _profileRepository.UpdateUserAsync(profileUser1);
        var result2 = await _profileRepository.UpdateUserAsync(profileUser2);

        if (!result1 || !result2)
            throw new ArgumentException("Database updating friends list of two users was occured exception");

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}