using MediatR;
using ProfileService.Application.Queries;
using ProfileService.Domain.Models.UserProfileAggregate.Repos;
using ProfileService.HttpModels.Responses;

namespace ProfileService.Application.Handlers;

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileResponse>
{
    private readonly IUserProfileRepository _userRepository;

    public GetUserProfileQueryHandler(IUserProfileRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<UserProfileResponse> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var result = await _userRepository.FindUserByEmailAsync(request.Email);

        return new UserProfileResponse()
        {
            UserName = result.UserName.Value,
            NickName = result.NickName.Value,
            Email = result.Email.Value
        };
    }
}