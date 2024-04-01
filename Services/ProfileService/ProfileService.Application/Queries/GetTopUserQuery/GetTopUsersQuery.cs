using DomainDriverDesignAbstractions;
using MediatR;
using ProfileService.Application.Models;

namespace ProfileService.Application.Queries.GetTopUserQuery;

public class GetTopUsersQuery : IRequest<Result<List<UserProfileInformation>>>
{
    public int Count { get; set; }
}