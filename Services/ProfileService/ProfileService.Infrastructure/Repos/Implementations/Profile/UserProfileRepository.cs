using Dapper;
using Npgsql;
using ProfileService.Domain.Models.Identity;
using ProfileService.Domain.Models.UserProfileAggregate;
using ProfileService.Domain.Models.UserProfileAggregate.Repos;
using ProfileService.Domain.Models.UserProfileAggregate.Snapshot;
using ProfileService.Infrastructure.Repos.Interfaces;

namespace ProfileService.Infrastructure.Repos.Implementations.Profile;

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
            .QueryAsync<UserProfileSnapshot, (string? profileId, string? friendId), UserProfileSnapshot>(
                ProfileQuery.SqlFindById, (profile, fl) =>
                {
                    if (fl.profileId is not null)
                    {
                        profile.FriendIds!.Add(fl.friendId!);
                    }
                    return profile;
                },
                splitOn: "ProfileId",
                param: parameters);

        var userDb = result.GroupBy(x => x.Id).Select(x =>
        {
            var userProfile = x.First();
            userProfile.FriendIds = x.SelectMany(usp =>
            {
                return usp.FriendIds!
                    .Where(friendId => !string.IsNullOrWhiteSpace(friendId))
                    .Select(friendId => friendId);
            }).ToList();

            return userProfile;
        }).FirstOrDefault();
        
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
            .QueryAsync<UserProfileSnapshot, (string? profileId, string? friendId), UserProfileSnapshot>(
                ProfileQuery.SqlFindByEmail, (profile, fl) =>
                {
                    if (fl.profileId is not null)
                    {
                        profile.FriendIds?.Add(fl.friendId!);
                    }
                    return profile;
                },
                splitOn: "ProfileId",
                param: parameters);

        var userDb = result.GroupBy(x => x.Id).Select(x =>
        {
            var userProfile = x.First();
            userProfile.FriendIds = x.SelectMany(usp =>
            {
                return usp.FriendIds!
                    .Where(friendId => !string.IsNullOrWhiteSpace(friendId))
                    .Select(friendId => friendId);
            }).ToList();

            return userProfile;
        }).FirstOrDefault();

        if (userDb == null) return null;

        var user = UserProfile.RestoreFromSnapshot(userDb);
        
        _tracker.Track(user);
        
        return user;
    }

    public async Task<bool> AddUserAsync(UserProfile user)
    {
        UserProfileSnapshot snapshot = user.Save();

        var command = new CommandDefinition(ProfileQuery.SqlAddUser, snapshot);

        var connection = await _factory.CreateConnection(default);
        var result = await connection.ExecuteAsync(command);

        return result == 1;
    }

    public async Task<bool> UpdateUserAsync(UserProfile user)
    {
        UserProfileSnapshot snapshot = user.Save();

        var command = new CommandDefinition(ProfileQuery.SqlUpdateUser, snapshot);

        var connection = await _factory.CreateConnection(default);
        var result = await connection.ExecuteAsync(command);

        var parameters = new { Id = user.Id.Value.ToString() };
        var list = await connection.QueryAsync<string>(FriendQuery.GetAllFriends, parameters);
        
        var friendIds = new HashSet<string>();
        foreach (var id in list)
            friendIds.Add(id);
        
        foreach (var friendId in user.Friends)
        {
            if (!friendIds.Contains(friendId.Value.ToString()))
            {
                // add to friends table
                var updateParameters = new
                {
                    Id = user.Id.Value.ToString(),
                    FriendId = friendId.Value.ToString() 
                };
                await connection.ExecuteAsync(FriendQuery.UpdateFriendsTable, updateParameters);
            }
        }

        foreach (var friendId in friendIds)
        {
            var friendIdResult = Id.Create(friendId).Value;
            if (!user.CheckIsFriend(friendIdResult))
            {
                // remove from friends table
                var removeParameters = new
                {
                    Id = user.Id.Value.ToString(),
                    FriendId = friendId 
                };
                await connection.ExecuteAsync(FriendQuery.DeleteFriendsFromTable, removeParameters);
            }
        }

        return result == 1;
    }

    public async Task<List<UserProfile>> GetAllUsers(int count)
    {
        var connection = await _factory.CreateConnection(default);

        var parameters = new
        {
            Count = count
        };

        IEnumerable<UserProfileSnapshot> result = await connection
            .QueryAsync<UserProfileSnapshot, (string? profileId, string? friendId), UserProfileSnapshot>(
                ProfileQuery.SqlGetAllUsers, (profile, fl) =>
                {
                    if (fl.profileId is not null)
                    {
                        profile.FriendIds?.Add(fl.friendId!);
                    }

                    return profile;
                },
                parameters,
                splitOn: "ProfileId");

        var dbUsers = result.GroupBy(x => x.Id).Select(x =>
        {
            var userProfile = x.First();
            userProfile.FriendIds = x.SelectMany(usp =>
            {
                return usp.FriendIds!
                    .Where(friendId => !string.IsNullOrWhiteSpace(friendId))
                    .Select(friendId => friendId);
            }).ToList();

            return userProfile;
        });

        List<UserProfile> profiles = new();
        foreach (var profile in dbUsers)
        {
            var domainProfile = UserProfile.RestoreFromSnapshot(profile);
            _tracker.Track(domainProfile);
            profiles.Add(domainProfile);
        }
        
        return profiles.OrderByDescending(p => p.Rating).Take(count).ToList();
    }
}