using DomainDriverDesignAbstractions;
using MediatR;
using ProfileService.Domain.Models.UserProfileAggregate.Errors;
using ProfileService.Domain.Models.UserProfileAggregate.Repos;

namespace ProfileService.Application.Commands.DeleteFromFriendsCommand;

public class DeleteFromFriendsCommandHandler
    : IRequestHandler<DeleteFromFriendsCommand, Result>
{
    private readonly IUserProfileRepository _profileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteFromFriendsCommandHandler(
        IUserProfileRepository profileRepository,
        IUnitOfWork unitOfWork)
    {
        _profileRepository = profileRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(DeleteFromFriendsCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.StartTransaction(cancellationToken);

        var profileSender = await _profileRepository.FindUserByEmailAsync(request.SenderEmail);
        var profileFriend = await _profileRepository.FindUserByEmailAsync(request.FriendEmail);
        if (profileSender is null || profileFriend is null)
            return Result.Failure(UserProfileErrors.FriendDoesNotExist);
        
        profileSender.DeleteFriend(profileFriend);
        profileFriend.DeleteFriend(profileSender);

        var result1 = await _profileRepository.UpdateUserAsync(profileSender);
        var result2 = await _profileRepository.UpdateUserAsync(profileFriend);
        
        if (!result1 || !result2)
            return Result.Failure(UserProfileErrors.CantRemoveUserFromFriends);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}