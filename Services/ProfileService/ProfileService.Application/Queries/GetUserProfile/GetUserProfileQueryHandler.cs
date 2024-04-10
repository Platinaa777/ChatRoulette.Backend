using DomainDriverDesignAbstractions;
using MediatR;
using ProfileService.Application.Models;
using ProfileService.Domain.Models.Identity;
using ProfileService.Domain.Models.UserProfileAggregate.Enumerations;
using ProfileService.Domain.Models.UserProfileAggregate.Errors;
using ProfileService.Domain.Models.UserProfileAggregate.Repos;

namespace ProfileService.Application.Queries.GetUserProfile;

public class GetUserProfileQueryHandler
    : IRequestHandler<GetUserProfileQuery, Result<ProfileResponse>>
{
    private readonly IUserProfileRepository _userRepository;

    public GetUserProfileQueryHandler(IUserProfileRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<Result<ProfileResponse>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var result = await _userRepository.FindUserByEmailAsync(request.Email);

        if (result == null)
            return Result.Failure<ProfileResponse>(UserProfileErrors.EmailNotFound);

        List<FriendInformation> friendsInformation = new();
        foreach (var friendId in result.Friends)
        {
            var friend = await _userRepository.FindUserByIdAsync(friendId.Value.ToString());
            
            if (friend is not null)
                friendsInformation.Add(new FriendInformation()
                {
                    Email = friend.Email.Value,
                    Nickname = friend.NickName.Value,
                    Rating = friend.Rating.Value
                });
        }
        
        return new ProfileResponse()
        {
            NickName = result.NickName.Value,
            Email = result.Email.Value,
            Age = result.Age.Value,
            Rating = result.Rating.Value,
            Friends = friendsInformation,
            Achivements = result.Achievements.Select(x => new AchivementResponse()
            {
                Title = AchievementType.FromValue(x.Id)!.Name,
                Content = AchievementContent.FromValue(x.Id)!.Name
            }).ToList()
        };
    }
}