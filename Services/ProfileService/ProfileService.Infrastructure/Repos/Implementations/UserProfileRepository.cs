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

    public UserProfileRepository(IDbConnectionFactory<NpgsqlConnection> factory)
    {
        _factory = factory;
    }
    
    public Task<UserProfile?> FindUserByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<UserProfile?> FindUserByEmailAsync(string email)
    {
        const string sql = $@"
            SELECT id, user_name UserName, nick_name NickName, email 
            FROM user_profiles
            WHERE email = @email;
         ";

        var parameters = new { Email = email };

        var connection = await _factory.CreateConnection(default);

        var result = await connection
            .QueryAsync<UserDb>(sql, param: parameters);

        var userDb = result.FirstOrDefault();

        var user = new UserProfile(
            userDb.Id,
            new Name(userDb.UserName),
            new Name(userDb.NickName),
            new Email(userDb.Email));
        
        return user;
    }

    public Task<bool> AddUserAsync(UserProfile user)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateUserAsync(UserProfile user)
    {
        throw new NotImplementedException();
    }
}