using MediatR;
using ProfileService.HttpModels.Responses;

namespace ProfileService.Application.Queries;

public class GetUserProfileQuery : IRequest<UserProfileResponse>
{
    public string Email { get; set; }
}