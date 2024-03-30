using MediatR;
using ProfileService.Domain.Models.FriendInvitationAggregate;
using ProfileService.Domain.Models.FriendInvitationAggregate.Errors;
using ProfileService.Domain.Models.FriendInvitationAggregate.Repos;
using ProfileService.Domain.Models.UserProfileAggregate.Errors;
using ProfileService.Domain.Models.UserProfileAggregate.Repos;
using ProfileService.Domain.Shared;

namespace ProfileService.Application.Commands.AcceptFriendInvitationCommand;

public class AcceptFriendInvitationCommandHandler
    : IRequestHandler<AcceptFriendInvitationCommand, Result>
{
    private readonly IUserProfileRepository _profileRepository;
    private readonly IFriendInvitationRepository _invitationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AcceptFriendInvitationCommandHandler(
        IUserProfileRepository profileRepository,
        IFriendInvitationRepository invitationRepository,
        IUnitOfWork unitOfWork)
    {
        _profileRepository = profileRepository;
        _invitationRepository = invitationRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(AcceptFriendInvitationCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.StartTransaction(cancellationToken);
        
        var firstProfile = await _profileRepository.FindUserByEmailAsync(request.InvitationSenderEmail);
        if (firstProfile is null)
            return Result.Failure(UserProfileErrors.EmailNotFound);
        
        var secondProfile = await _profileRepository.FindUserByEmailAsync(request.InvitationSenderEmail);
        if (secondProfile is null)
            return Result.Failure(UserProfileErrors.EmailNotFound);
        
        // cant accept because they are already friends
        if (firstProfile.CheckIsFriend(secondProfile.Id))
        {
            return Result.Failure(InvitationErrors.AlreadyFriends);
        }

        var invitationBetweenUsers = await _invitationRepository.FindByProfileIds(
            firstProfile.Id,
            secondProfile.Id);
        
        if (invitationBetweenUsers is null)
            return Result.Failure(InvitationErrors.InvitationDoesNotExist);

        if (invitationBetweenUsers.IsHandled())
        {
            return Result.Failure(InvitationErrors.InvalidOperation);
        }
        
        invitationBetweenUsers.SetAccepted();
        var response = await _invitationRepository.Update(invitationBetweenUsers);
        if (!response)
            return Result.Failure(InvitationErrors.CantUpdateInvitationStatus);

        var removeResult = await _invitationRepository.Remove(invitationBetweenUsers.Id);
        if (!removeResult)
            return Result.Failure(InvitationErrors.CantUpdateInvitationStatus);
        
        // implement outbox pattern for transactional scope for aggregates (1 transaction ~ 1 aggregate root)
        
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}