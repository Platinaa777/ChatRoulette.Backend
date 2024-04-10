using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using ProfileService.Domain.Models.UserProfileAggregate;
using ProfileService.Domain.Models.UserProfileAggregate.Repos;
using ProfileService.Domain.Models.UserProfileAggregate.Snapshot;
using ProfileService.Infrastructure.Repos.Implementations.Profile;

namespace ProfileService.Infrastructure.Cache;

public class CacheUserProfileRepository : IUserProfileRepository
{
    private readonly IDistributedCache _cache;
    private readonly UserProfileRepository _profileRepository;

    public CacheUserProfileRepository(
        IDistributedCache cache,
        UserProfileRepository profileRepository)
    {
        _cache = cache;
        _profileRepository = profileRepository;
    }
    
    public async Task<UserProfile?> FindUserByIdAsync(string id)
    {
        string key = $"profile-id-{id}";
        
        var cachedProfile = await _cache.GetStringAsync(key);
        UserProfile? userProfile;

        if (cachedProfile is not null)
            return UserProfile.RestoreFromSnapshot(JsonConvert.DeserializeObject<UserProfileSnapshot>(cachedProfile,
                new JsonSerializerSettings()
                {
                    ContractResolver = new PrivateContractResolver(),
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
                })!);

        userProfile = await _profileRepository.FindUserByIdAsync(id);
        if (userProfile is null)
            return userProfile;

        await _cache.SetStringAsync(key, JsonConvert.SerializeObject(userProfile.Save()), new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
        });

        return userProfile;
    }

    public async Task<UserProfile?> FindUserByEmailAsync(string email)
    {
        string key = $"profile-email-{email}";
        
        var cachedProfile = await _cache.GetStringAsync(key);
        UserProfile? userProfile;

        if (cachedProfile is not null)
            return UserProfile.RestoreFromSnapshot(JsonConvert.DeserializeObject<UserProfileSnapshot>(cachedProfile,
                new JsonSerializerSettings()
                {
                    ContractResolver = new PrivateContractResolver(),
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
                })!);

        userProfile = await _profileRepository.FindUserByEmailAsync(email);
        if (userProfile is null)
            return userProfile;

        await _cache.SetStringAsync(key, JsonConvert.SerializeObject(userProfile.Save()));

        return userProfile;
    }

    public async Task<bool> AddUserAsync(UserProfile user)
    {
        string key = $"profile-id-{user.Id.Value.ToString()}";

        await _cache.SetStringAsync(key, JsonConvert.SerializeObject(user.Save()), new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
        });
        
        return await _profileRepository.AddUserAsync(user);
    }

    public async Task<bool> UpdateUserAsync(UserProfile user)
    {
        string key = $"profile-id-{user.Id.Value.ToString()}";

        await _cache.SetStringAsync(key, JsonConvert.SerializeObject(user.Save()), new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
        });
        
        return await _profileRepository.UpdateUserAsync(user);
    }

    public async Task<List<UserProfile>> GetAllUsers(int count)
    {
        return await _profileRepository.GetAllUsers(count);
    }
}