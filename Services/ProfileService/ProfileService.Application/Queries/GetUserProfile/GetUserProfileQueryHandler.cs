using DomainDriverDesignAbstractions;
using MediatR;
using ProfileService.Application.Models;
using ProfileService.Domain.Models.Identity;
using ProfileService.Domain.Models.UserProfileAggregate;
using ProfileService.Domain.Models.UserProfileAggregate.Enumerations;
using ProfileService.Domain.Models.UserProfileAggregate.Errors;
using ProfileService.Domain.Models.UserProfileAggregate.Repos;
using S3.Client;

namespace ProfileService.Application.Queries.GetUserProfile;

public class GetUserProfileQueryHandler
    : IRequestHandler<GetUserProfileQuery, Result<ProfileResponse>>
{
    private readonly IUserProfileRepository _userRepository;
    private readonly IS3Client _s3Client;

    public GetUserProfileQueryHandler(
        IUserProfileRepository userRepository,
        IS3Client s3Client)
    {
        _userRepository = userRepository;
        _s3Client = s3Client;
    }
    
    public async Task<Result<ProfileResponse>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var result = await _userRepository.FindUserByEmailAsync(request.Email);

        if (result == null)
            return Result.Failure<ProfileResponse>(UserProfileErrors.EmailNotFound);

        return await GetUserProfile(result);
    }

    private async Task<ProfileResponse> GetUserProfile(UserProfile result)
    {
        List<FriendInformation> friendsInformation = new();
        foreach (var friendId in result.Friends)
        {
            var friend = await _userRepository.FindUserByIdAsync(friendId.Value.ToString());
            
            if (friend is not null)
                friendsInformation.Add(new FriendInformation()
                {
                    Email = friend.Email.Value,
                    Nickname = friend.UserName.Value,
                    Rating = friend.Rating.Value
                });
        }
        
        List<AchievementResponse> achievementResponses = new();
        foreach (var achievement in result.Achievements)
        {
            if (AchievementType.FromValue(achievement.Id) is null)
                continue;
            
            var photoUrl = await _s3Client.FindFileAsync(
                bucket: "achievement-s3-objects",
                filename: achievement.Id + ".jpg");
            
            achievementResponses.Add(new AchievementResponse()
            {
                Title = AchievementType.FromValue(achievement.Id)!.Name,
                Content = AchievementContent.FromValue(achievement.Id)!.Name,
                Url = photoUrl?.Link ?? string.Empty
            });
        }
        
        return new ProfileResponse()
        {
            UserName = result.UserName.Value,
            Email = result.Email.Value,
            BirthDateUtc = result.BirthDateUtc.Value,
            Rating = result.Rating.Value,
            Friends = friendsInformation,
            Avatar = result.Avatar.Value,
            Achievements = achievementResponses
        };
    }
}