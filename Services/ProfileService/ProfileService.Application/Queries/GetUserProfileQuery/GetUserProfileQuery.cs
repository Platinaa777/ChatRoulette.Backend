using MediatR;
using ProfileService.Domain.Shared;
using ProfileService.HttpModels.Responses;

namespace ProfileService.Application.Queries.GetUserProfileQuery;

public class GetUserProfileQuery : IRequest<Result<ProfileResponse>>
{
    public string Email { get; set; }
}