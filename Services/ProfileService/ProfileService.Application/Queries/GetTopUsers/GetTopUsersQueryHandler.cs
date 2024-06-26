using DomainDriverDesignAbstractions;
using MediatR;
using Microsoft.Net.Http.Headers;
using ProfileService.Application.Models;
using ProfileService.Domain.Models.UserProfileAggregate.Repos;

namespace ProfileService.Application.Queries.GetTopUsers;

public class GetTopUsersQueryHandler
    : IRequestHandler<GetTopUsersQuery, Result<List<UserProfileInformation>>>
{
    private readonly IUserProfileRepository _profileRepository;

    public GetTopUsersQueryHandler(IUserProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }
    
    public async Task<Result<List<UserProfileInformation>>> Handle(GetTopUsersQuery request, CancellationToken cancellationToken)
    {
        var result = await _profileRepository.GetAllUsers(request.Count);

        List<UserProfileInformation> profiles = new();
        foreach (var profile in result)
        {
            profiles.Add(new UserProfileInformation() { 
                UserName = profile.UserName.Value,
                Rating = profile.Rating.Value,
                Avatar = profile.Avatar.Value
            });
        }

        return profiles;
    }
}