using DomainDriverDesignAbstractions;
using MediatR;
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
    
    public async Task<Result<ProfileResponse>> Handle(GetUserProfile.GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var result = await _userRepository.FindUserByEmailAsync(request.Email);

        if (result == null)
            return Result.Failure<ProfileResponse>(UserProfileErrors.EmailNotFound);
        
        return new ProfileResponse()
        {
            NickName = result.NickName.Value,
            Email = result.Email.Value,
            Age = result.Age.Value,
            Rating = result.Rating.Value,
            FriendIds = result.Friends.Select(x => x.Value.ToString()).ToList(),
            Achivements = result.Achievements.Select(x => new AchivementResponse()
            {
                Title = AchievementType.FromValue(x.Id)!.Name,
                Content = AchievementContent.FromValue(x.Id)!.Name
            }).ToList()
        };
    }
}