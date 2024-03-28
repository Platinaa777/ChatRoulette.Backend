using MediatR;
using ProfileService.Application.Models;
using ProfileService.Domain.Shared;

namespace ProfileService.Application.Queries.GetTopUserQuery;

public class GetTopUsersQuery : IRequest<Result<List<UserProfileInformation>>>
{
    public int Count { get; set; }
}