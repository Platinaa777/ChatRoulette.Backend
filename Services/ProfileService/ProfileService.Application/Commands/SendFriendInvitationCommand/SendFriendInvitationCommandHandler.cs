using DomainDriverDesignAbstractions;
using MediatR;
using ProfileService.Domain.Models.FriendInvitationAggregate;
using ProfileService.Domain.Models.FriendInvitationAggregate.Enumerations;
using ProfileService.Domain.Models.FriendInvitationAggregate.Errors;
using ProfileService.Domain.Models.FriendInvitationAggregate.Repos;
using ProfileService.Domain.Models.UserProfileAggregate.Errors;
using ProfileService.Domain.Models.UserProfileAggregate.Repos;

namespace ProfileService.Application.Commands.SendFriendInvitationCommand;

public class SendFriendInvitationCommandHandler
    : IRequestHandler<SendFriendInvitationCommand, Result>
{
    private readonly IUserProfileRepository _profileRepository;
    private readonly IFriendInvitationRepository _invitationRepository;

    public SendFriendInvitationCommandHandler(
        IUserProfileRepository profileRepository,
        IFriendInvitationRepository invitationRepository)
    {
        _profileRepository = profileRepository;
        _invitationRepository = invitationRepository;
    }
    
    public async Task<Result> Handle(Commands.SendFriendInvitationCommand.SendFriendInvitationCommand request, CancellationToken cancellationToken)
    {
        var firstProfile = await _profileRepository.FindUserByEmailAsync(request.InvitationSenderEmail);
        if (firstProfile is null)
            return Result.Failure(UserProfileErrors.EmailNotFound);
        
        var secondProfile = await _profileRepository.FindUserByEmailAsync(request.InvitationReceiverEmail);
        if (secondProfile is null)
            return Result.Failure(UserProfileErrors.EmailNotFound);

        if (firstProfile.CheckIsFriend(secondProfile.Id))
        {
            return Result.Failure(InvitationErrors.AlreadyFriends);
        }

        var isHasInvitation = await _invitationRepository.FindByProfileIds(
            firstProfile.Id, 
            secondProfile.Id);
        
        if (isHasInvitation is not null)
            return Result.Failure(InvitationErrors.InvitationAlreadyExist);

        var invitationResult = FriendInvitation.Create(
            id: Guid.NewGuid().ToString(),
            firstProfile.Id.Value.ToString(),
            secondProfile.Id.Value.ToString(),
            InvitationStatus.Pending.Name,
            DateTime.Now);

        if (invitationResult.IsFailure)
            return Result.Failure(invitationResult.Error);

        var result = await _invitationRepository.Add(invitationResult.Value);
        if (!result)
            return Result.Failure(InvitationErrors.CantAddInvitation);

        return Result.Success();
    }
}