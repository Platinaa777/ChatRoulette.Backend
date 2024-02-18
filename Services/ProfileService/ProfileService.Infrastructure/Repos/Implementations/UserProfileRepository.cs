using Dapper;
using Npgsql;
using ProfileService.Domain.Models.UserProfileAggregate.Entities;
using ProfileService.Domain.Models.UserProfileAggregate.Repos;
using ProfileService.Domain.Models.UserProfileAggregate.ValueObjects;
using ProfileService.Infrastructure.Repos.ConnectionFactories;
using ProfileService.Infrastructure.Repos.Interfaces;
using ProfileService.Infrastructure.Repos.Models;

namespace ProfileService.Infrastructure.Repos.Implementations;

public class UserProfileRepository : IUserProfileRepository
{
    private readonly IDbConnectionFactory<NpgsqlConnection> _factory;
    private readonly IChangeTracker _tracker;

    public UserProfileRepository(IDbConnectionFactory<NpgsqlConnection> factory, IChangeTracker tracker)
    {
        _factory = factory;
        _tracker = tracker;
    }
    
    public async Task<UserProfile?> FindUserByIdAsync(string id)
    {
        var parameters = new { Id = id };

        var connection = await _factory.CreateConnection(default);
        
        IEnumerable<UserDb> result = await connection
            .QueryAsync<UserDb>(NpgsqlQuery.sqlFindById, 
                param: parameters);

        var userDb = result.FirstOrDefault();
        
        if (userDb == null) return null;
        
        var user = new UserProfile(
            userDb.Id,
            new Name(userDb.UserName),
            new Name(userDb.NickName),
            new Email(userDb.Email));
        
        _tracker.Track(user);

        return user;
    }

    public async Task<UserProfile?> FindUserByEmailAsync(string email)
    {
        var parameters = new { Email = email };

        var connection = await _factory.CreateConnection(default);

        IEnumerable<UserDb> result = await connection
                                .QueryAsync<UserDb>(NpgsqlQuery.sqlFindByEmail, 
                                                    param: parameters);

        var userDb = result.FirstOrDefault();

        if (userDb == null) return null;

        var user = new UserProfile(
            userDb.Id,
            new Name(userDb.UserName),
            new Name(userDb.NickName),
            new Email(userDb.Email));
        
        _tracker.Track(user);
        
        return user;
    }

    public async Task<bool> AddUserAsync(UserProfile user)
    {
        var userDb = await FindUserByEmailAsync(user.Email.Value);

        if (userDb != null) return false;

        var parameteres = new
        {
            Id = user.Id,
            UserName = user.UserName.Value,
            NickName = user.NickName.Value,
            Email = user.Email.Value
        };

        var command = new CommandDefinition(NpgsqlQuery.sqlAddUser, parameteres);

        var connection = await _factory.CreateConnection(default);
        var result = await connection.ExecuteAsync(command);

        return result == 1;
    }

    public async Task<bool> UpdateUserAsync(UserProfile user)
    {
        var userDb = await FindUserByEmailAsync(user.Email.Value);

        if (userDb != null) return false;

        var parameteres = new
        {
            UserName = user.UserName.Value,
            NickName = user.NickName.Value,
            Email = user.Email.Value
        };

        var command = new CommandDefinition(NpgsqlQuery.sqlUpdateUser, parameteres);

        var connection = await _factory.CreateConnection(default);
        var result = await connection.ExecuteAsync(command);

        return result == 1;
    }
}