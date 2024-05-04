using AuthService.Application.Cache;
using AuthService.Application.Constants;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AuthService.Infrastructure.Cache;

public class RedisCache : ICacheStorage 
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<RedisCache> _logger;

    public RedisCache(
        IDistributedCache cache,
        ILogger<RedisCache> logger)
    {
        _cache = cache;
        _logger = logger;
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

        var date = DateTime.UtcNow.AddHours(1);
        await _cache.SetStringAsync(key + CacheConstants.Delete, JsonConvert.SerializeObject(date), cancellationToken);
        _logger.LogInformation("User with {@Key} will be deleted at {@Date}", key, date);
    }
}