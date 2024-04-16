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
    
    public async Task<List<InvitationResponse>> GetAllUserInvitations(string email, CancellationToken cancellationToken)
    {
        var connection = await _factory.CreateConnectionAsync(cancellationToken);

        var parameters = new
        {
            Email = email
        };
        
        IEnumerable<InvitationResponse> result = await connection
            .QueryAsync<InvitationResponse>(
                @"
                    SELECT email as Email, user_name as UserName, avatar as Avatar
                    FROM user_profiles LEFT JOIN invitations ON user_profile.id = invitations.receiver_id
                    WHERE email = @email;
                ", 
                param: parameters);

        return result.ToList();

    }
}