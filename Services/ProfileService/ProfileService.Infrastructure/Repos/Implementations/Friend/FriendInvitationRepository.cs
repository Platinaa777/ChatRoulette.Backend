using Dapper;
using Npgsql;
using ProfileService.Domain.Models.FriendInvitationAggregate;
using ProfileService.Domain.Models.FriendInvitationAggregate.Repos;
using ProfileService.Domain.Models.FriendInvitationAggregate.Snapshot;
using ProfileService.Domain.Models.Identity;
using ProfileService.Infrastructure.PersistenceAbstractions;

namespace ProfileService.Infrastructure.Repos.Implementations.Friend;

public class FriendInvitationRepository : IFriendInvitationRepository
{
    private readonly IDbConnectionFactory<NpgsqlConnection> _factory;
    private readonly IChangeTracker _tracker;

    public FriendInvitationRepository(
        IDbConnectionFactory<NpgsqlConnection> factory,
        IChangeTracker tracker)
    {
        _factory = factory;
        _tracker = tracker;
    }
    
    public async Task<FriendInvitation?> FindByInvitationId(Id id)
    {
        var parameters = new { Id = id };

        var connection = await _factory.CreateConnectionAsync(default);
        
        IEnumerable<FriendInvitationSnapshot> result = await connection
            .QueryAsync<FriendInvitationSnapshot>(FriendInvitationQuery.SqlFindById, 
                param: parameters);

        var invitationSnapshot = result.FirstOrDefault();
        
        if (invitationSnapshot == null) 
            return null;

        var friendInvitation = FriendInvitation.RestoreFromSnapshot(invitationSnapshot);
        
        _tracker.Track(friendInvitation);

        return friendInvitation;
    }

    public async Task<FriendInvitation?> FindByProfileIds(Id senderId, Id receiverId)
    {
        var parameters = new
        {
            SenderId = senderId.Value.ToString(),
            ReceiverId = receiverId.Value.ToString()
        };

        var connection = await _factory.CreateConnectionAsync(default);
        
        IEnumerable<FriendInvitationSnapshot> result = await connection
            .QueryAsync<FriendInvitationSnapshot>(FriendInvitationQuery.SqlFindByProfileIds, 
                param: parameters);

        var invitationSnapshot = result.FirstOrDefault();
        
        if (invitationSnapshot == null) 
            return null;

        var friendInvitation = FriendInvitation.RestoreFromSnapshot(invitationSnapshot);
        
        _tracker.Track(friendInvitation);

        return friendInvitation;
    }
    
    public async Task<bool> Add(FriendInvitation invitation)
    {
        FriendInvitationSnapshot snapshot = invitation.Save();

        var command = new CommandDefinition(FriendInvitationQuery.SqlAddInvitation, snapshot);

        var connection = await _factory.CreateConnectionAsync(default);
        var result = await connection.ExecuteAsync(command);

        return result == 1;
    }

    public async Task<bool> Update(FriendInvitation invitation)
    {
        FriendInvitationSnapshot snapshot = invitation.Save();

        var command = new CommandDefinition(FriendInvitationQuery.SqlUpdateInvitation, snapshot);

        var connection = await _factory.CreateConnectionAsync(default);
        var result = await connection.ExecuteAsync(command);

        return result == 1;
    }

    public async Task<bool> Remove(Id invitationId)
    {
        var parameters = new
        {
            Id = invitationId.Value.ToString()
        };

        var connection = await _factory.CreateConnectionAsync(default);
        var command = new CommandDefinition(FriendInvitationQuery.SqlDeleteInvitation, parameters);

        var result = await connection.ExecuteAsync(command);

        return result == 1;
    }
}