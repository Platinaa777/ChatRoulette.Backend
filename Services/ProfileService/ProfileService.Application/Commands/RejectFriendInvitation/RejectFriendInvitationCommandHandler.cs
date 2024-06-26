using DomainDriverDesignAbstractions;
using MediatR;
using ProfileService.Domain.Models.FriendInvitationAggregate.Errors;
using ProfileService.Domain.Models.FriendInvitationAggregate.Repos;
using ProfileService.Domain.Models.UserProfileAggregate.Errors;
using ProfileService.Domain.Models.UserProfileAggregate.Repos;

namespace ProfileService.Application.Commands.RejectFriendInvitation;

public class RejectFriendInvitationCommandHandler
    : IRequestHandler<RejectFriendInvitation.RejectFriendInvitationCommand, Result>
{
    private readonly IUserProfileRepository _profileRepository;
    private readonly IFriendInvitationRepository _invitationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RejectFriendInvitationCommandHandler(
        IUserProfileRepository profileRepository,
        IFriendInvitationRepository invitationRepository,
        IUnitOfWork unitOfWork)
    {
        _profileRepository = profileRepository;
        _invitationRepository = invitationRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(RejectFriendInvitation.RejectFriendInvitationCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.StartTransaction(cancellationToken);
        
        var firstProfile = await _profileRepository.FindUserByEmailAsync(request.AnswerEmail);
        if (firstProfile is null)
            return Result.Failure(UserProfileErrors.EmailNotFound);
        
        var secondProfile = await _profileRepository.FindUserByEmailAsync(request.SenderEmail);
        if (secondProfile is null)
            return Result.Failure(UserProfileErrors.EmailNotFound);
        
        // cant reject because already they are friend 
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
        
        await _invitationRepository.Remove(invitationBetweenUsers.Id);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}