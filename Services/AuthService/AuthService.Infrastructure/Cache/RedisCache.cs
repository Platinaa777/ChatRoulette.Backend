using AuthService.Application.Cache;
using Microsoft.Extensions.Caching.Distributed;

namespace AuthService.Infrastructure.Cache;

public class RedisCache : ICacheStorage 
{
    private readonly IDistributedCache _cache;

    public RedisCache(IDistributedCache cache)
    {
        _cache = cache;
    }
    
    public async Task<string?> GetAsync(string key, CancellationToken cancellationToken = default)
    {
        return await _cache.GetStringAsync(key, cancellationToken);
    }

    public async Task SetAsync(string key, string value, CancellationToken cancellationToken = default)
    {
        await _cache.SetStringAsync(key, value, new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
        }, cancellationToken);

    }
}