using Dapper;
using Newtonsoft.Json;
using Npgsql;
using ProfileService.Domain.Models.UserProfileAggregate;
using ProfileService.Domain.Models.UserProfileAggregate.Repos;
using ProfileService.Domain.Models.UserProfileAggregate.Snapshot;
using ProfileService.Domain.Models.UserProfileAggregate.ValueObjects;
using ProfileService.Infrastructure.Repos.Interfaces;
using ProfileService.Infrastructure.Repos.Models;

namespace ProfileService.Infrastructure.Repos.Implementations;

public class UserProfileRepository : IUserProfileRepository
{
    private readonly IDbConnectionFactory<NpgsqlConnection> _factory;
    private readonly IChangeTracker _tracker;

    public UserProfileRepository(
        IDbConnectionFactory<NpgsqlConnection> factory,
        IChangeTracker tracker)
    {
        _factory = factory;
        _tracker = tracker;
    }
    
    public async Task<UserProfile?> FindUserByIdAsync(string id)
    {
        var parameters = new { Id = id };

        var connection = await _factory.CreateConnection(default);
        
        IEnumerable<UserProfileSnapshot> result = await connection
            .QueryAsync<UserProfileSnapshot>(NpgsqlQuery.SqlFindById, 
                param: parameters);

        var userDb = result.FirstOrDefault();
        
        if (userDb == null) 
            return null;

        var user = UserProfile.RestoreFromSnapshot(userDb);
        
        _tracker.Track(user);

        return user;
    }

    public async Task<UserProfile?> FindUserByEmailAsync(string email)
    {
        var parameters = new { Email = email };

        var connection = await _factory.CreateConnection(default);

        IEnumerable<UserProfileSnapshot> result = await connection
                                .QueryAsync<UserProfileSnapshot>(NpgsqlQuery.SqlFindByEmail, 
                                                    param: parameters);

        var userDb = result.FirstOrDefault();

        if (userDb == null) return null;

        var user = UserProfile.RestoreFromSnapshot(userDb);
        
        _tracker.Track(user);
        
        return user;
    }

    public async Task<bool> AddUserAsync(UserProfile user)
    {
        var parameters = new
        {
            Id = user.Id,
            NickName = user.NickName.Value,
            Email = user.Email.Value,
            Age = user.Age.Value,
            Rating = user.Rating.Value,
            Friends = JsonConvert.SerializeObject(user.Friends.Select(x => x.Value.ToString()))
        };

        var command = new CommandDefinition(NpgsqlQuery.SqlAddUser, parameters);

        var connection = await _factory.CreateConnection(default);
        var result = await connection.ExecuteAsync(command);

        return result == 1;
    }

    public async Task<bool> UpdateUserAsync(UserProfile user)
    {
        var parameters = new
        {
            Id = user.Id,
            NickName = user.NickName.Value,
            Email = user.Email.Value,
            Age = user.Age.Value,
            Rating = user.Rating.Value,
            Friends = JsonConvert.SerializeObject(user.Friends.Select(x => x.Value.ToString()))
        };

        var command = new CommandDefinition(NpgsqlQuery.SqlUpdateUser, parameters);

        var connection = await _factory.CreateConnection(default);
        var result = await connection.ExecuteAsync(command);

        return result == 1;
    }

    public async Task<List<UserProfile>> GetAllUsers()
    {
        var connection = await _factory.CreateConnection(default);
        
        IEnumerable<UserProfileSnapshot> result = await connection
            .QueryAsync<UserProfileSnapshot>(NpgsqlQuery.SqlGetAllUsers);

        List<UserProfile> profiles = new();
        foreach (var profile in result)
        {
            var domainProfile = UserProfile.RestoreFromSnapshot(profile);
            _tracker.Track(domainProfile);
            profiles.Add(domainProfile);
        }
        
        return profiles;
    }
}