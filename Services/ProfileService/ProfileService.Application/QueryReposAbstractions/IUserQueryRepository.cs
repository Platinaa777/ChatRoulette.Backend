using ProfileService.Application.Models;

namespace ProfileService.Application.QueryReposAbstractions;

public interface IUserQueryRepository
{
    Task<List<InvitationSenderId>> GetAllUserInvitations(string id, CancellationToken cancellationToken = default);
    Task<RecentPeerInformation?> GetRecentPeersInformation(string peerEmail, CancellationToken cancellationToken = default);
}