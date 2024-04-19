using DomainDriverDesignAbstractions;
using MediatR;
using ProfileService.Application.Models;
using ProfileService.Application.QueryReposAbstractions;
using ProfileService.Domain.Models.FriendInvitationAggregate.Errors;
using ProfileService.Domain.Models.UserProfileAggregate.Errors;
using ProfileService.Domain.Models.UserProfileAggregate.Repos;

namespace ProfileService.Application.Queries.GetInvitations;

public class GetInvitationsQueryHandler
    : IRequestHandler<GetInvitationsQuery, Result<List<InvitationResponse>>>
{
    private readonly IUserProfileRepository _profileRepository;
    private readonly IUserQueryRepository _userQueryRepository;

    public GetInvitationsQueryHandler(
        IUserProfileRepository profileRepository,
        IUserQueryRepository userQueryRepository)
    {
        _profileRepository = profileRepository;
        _userQueryRepository = userQueryRepository;
    }
    
    public async Task<Result<List<InvitationResponse>>> Handle(GetInvitationsQuery request, CancellationToken cancellationToken)
    {
        var user = await _profileRepository.FindUserByEmailAsync(request.Email);

        if (user is null)
            return Result.Failure<List<InvitationResponse>>(UserProfileErrors.UserNotFound);

        var invitations = await _userQueryRepository.GetAllUserInvitations(user.Id.Value.ToString(), cancellationToken);

        if (invitations.Count == 0)
            return Result.Failure<List<InvitationResponse>>(InvitationErrors.NoInvitations);

        List<InvitationResponse> invitationResponses = new();

        foreach (var invitation in invitations)
        {
            var userInfo = await _profileRepository.FindUserByIdAsync(invitation.SenderId);

            if (userInfo is not null)
            {
                invitationResponses.Add(new InvitationResponse()
                {
                    Avatar = userInfo.Avatar.Value,
                    Email = userInfo.Email.Value,
                    UserName = userInfo.UserName.Value,
                    Rating = userInfo.Rating.Value
                });
            }
        }
        
        return invitationResponses;
    }
}