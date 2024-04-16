using ProfileService.Application.Models;

namespace ProfileService.Application.QueryReposAbstractions;

public interface IUserQueryRepository
{
    Task<List<InvitationResponse>> GetAllUserInvitations(string email, CancellationToken cancellationToken = default);
    Task<RecentPeerInformation?> GetRecentPeersInformation(string peerEmail, CancellationToken cancellationToken = default);
}