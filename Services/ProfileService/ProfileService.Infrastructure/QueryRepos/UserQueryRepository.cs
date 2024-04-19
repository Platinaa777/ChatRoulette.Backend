using Dapper;
using Npgsql;
using ProfileService.Application.Models;
using ProfileService.Application.QueryReposAbstractions;
using ProfileService.Domain.Models.UserProfileAggregate.Snapshot;
using ProfileService.Infrastructure.PersistenceAbstractions;
using ProfileService.Infrastructure.Repos.Implementations.Profile;

namespace ProfileService.Infrastructure.QueryRepos;

public class UserQueryRepository : IUserQueryRepository
{
    private readonly IDbConnectionFactory<NpgsqlConnection> _factory;

    public UserQueryRepository(
        IDbConnectionFactory<NpgsqlConnection> factory)
    {
        _factory = factory;
    }
    
    public async Task<List<InvitationSenderId>> GetAllUserInvitations(string id, CancellationToken cancellationToken)
    {
        var connection = await _factory.CreateConnectionAsync(cancellationToken);

        var parameters = new
        {
            Id = id
        };
        
        IEnumerable<InvitationSenderId> result = await connection
            .QueryAsync<InvitationSenderId>(
                @"
                    SELECT sender_id As SenderId
                    FROM invitations
                    WHERE receiver_id = @Id;
                ", 
                param: parameters);

        return result.ToList();

    }

    public async Task<RecentPeerInformation?> GetRecentPeersInformation(string peerEmail, CancellationToken cancellationToken = default)
    {
        var connection = await _factory.CreateConnectionAsync(cancellationToken);

        var parameters = new
        {
            PeerEmail = peerEmail
        };
        
        IEnumerable<RecentPeerInformation> result = await connection
            .QueryAsync<RecentPeerInformation>(
                @"
                    SELECT email as Email, user_name as UserName, rating as Rating, avatar as Avatar
                    FROM user_profiles
                    WHERE email = @PeerEmail;
                ", 
                param: parameters);

        return result.FirstOrDefault();
    }
}