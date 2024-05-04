using DomainDriverDesignAbstractions;
using MediatR;
using ProfileService.Application.Models;
using ProfileService.Application.QueryReposAbstractions;
using ProfileService.Domain.Models.UserProfileAggregate.Errors;

namespace ProfileService.Application.Queries.GetPeersInformation;

public class GetPeersInformationQueryHandler
    : IRequestHandler<GetPeersInformationQuery, Result<List<RecentPeerInformation>>>
{
    private readonly IUserQueryRepository _userQueryRepository;

    public GetPeersInformationQueryHandler(
        IUserQueryRepository userQueryRepository)
    {
        _userQueryRepository = userQueryRepository;
    }
    
    public async Task<Result<List<RecentPeerInformation>>> Handle(GetPeersInformationQuery request, CancellationToken cancellationToken)
    {
        var recentPeers = new List<RecentPeerInformation>();

        foreach (var email in request.PeerEmails)
        {
            var user = await _userQueryRepository.GetRecentPeersInformation(email, cancellationToken);
            
            if (user is not null)
                recentPeers.Add(new RecentPeerInformation()
                {
                    Email = email,
                    Avatar = user.Avatar,
                    Rating = user.Rating,
                    UserName = user.UserName
                });
        }

        return recentPeers;
    }
}