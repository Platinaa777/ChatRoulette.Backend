using DomainDriverDesignAbstractions;
using MediatR;
using ProfileService.Application.Models;

namespace ProfileService.Application.Queries.GetTopUsers;

public class GetTopUsersQuery : IRequest<Result<List<UserProfileInformation>>>
{
    public GetTopUsersQuery(int count)
    {
        Count = count;
    }

    public int Count { get; set; }
    
}