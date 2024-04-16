using Dapper;
using Npgsql;
using ProfileService.Domain.Models.Identity;
using ProfileService.Domain.Models.UserHistoryAggregate;
using ProfileService.Domain.Models.UserHistoryAggregate.Repos;
using ProfileService.Domain.Models.UserHistoryAggregate.Snapshots;
using ProfileService.Infrastructure.PersistenceAbstractions;

namespace ProfileService.Infrastructure.Repos.Implementations.History;

public class UserHistoryRepository : IUserHistoryRepository
{
    private readonly IDbConnectionFactory<NpgsqlConnection> _factory;
    private readonly IChangeTracker _tracker;

    public UserHistoryRepository(
        IDbConnectionFactory<NpgsqlConnection> factory,
        IChangeTracker tracker)
    {
        _factory = factory;
        _tracker = tracker;
    }
    
    public async Task<UserHistory?> FindByUserId(Id id)
    {
        var parameters = new { UserId = id.Value.ToString() };

        var connection = await _factory.CreateConnectionAsync(default);
        
        IEnumerable<UserHistorySnapshot> result = await connection
            .QueryAsync<UserHistorySnapshot>(
                HistoryQuery.SqlFindByUserId, 
                param: parameters);

        var userHistorySnapshot = result.FirstOrDefault();
        
        if (userHistorySnapshot == null) 
            return null;

        var history = UserHistory.RestoreFromSnapshot(userHistorySnapshot);
        
        _tracker.Track(history);

        return history;
    }

    public async Task<UserHistory?> FindByHistoryId(Id id)
    {
        var parameters = new { Id = id.Value.ToString() };

        var connection = await _factory.CreateConnectionAsync(default);
        
        IEnumerable<UserHistorySnapshot> result = await connection
            .QueryAsync<UserHistorySnapshot>(
                HistoryQuery.SqlFindByHistoryId, 
                param: parameters);

        var userHistorySnapshot = result.FirstOrDefault();
        
        if (userHistorySnapshot == null) 
            return null;

        var history = UserHistory.RestoreFromSnapshot(userHistorySnapshot);
        
        _tracker.Track(history);

        return history;
    }

    public async Task<bool> AddHistory(UserHistory history)
    {
        UserHistorySnapshot snapshot = history.Save();

        var command = new CommandDefinition(HistoryQuery.SqlAddHistory, snapshot);

        var connection = await _factory.CreateConnectionAsync(default);
        var result = await connection.ExecuteAsync(command);

        return result == 1;
    }

    public async Task<bool> UpdateHistory(UserHistory history)
    {
        UserHistorySnapshot snapshot = history.Save();

        var command = new CommandDefinition(HistoryQuery.SqlUpdateUpdate, snapshot);

        var connection = await _factory.CreateConnectionAsync(default);
        var result = await connection.ExecuteAsync(command);

        return result == 1;
    }
}