using AuthService.DataContext.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Quartz;

namespace AuthService.Api.BackgroundJobs;

[DisallowConcurrentExecution]
public class DeleteUnactivatedUserJob : IJob
{
    private readonly UserDb _dbContext;
    private readonly IDistributedCache _cache;
    private readonly ILogger<DeleteUnactivatedUserJob> _logger;

    public DeleteUnactivatedUserJob(
        UserDb dbContext,
        IDistributedCache cache,
        ILogger<DeleteUnactivatedUserJob> logger)
    {
        _dbContext = dbContext;
        _cache = cache;
        _logger = logger;
    }    
    
    public async Task Execute(IJobExecutionContext context)
    {
        var unactivatedUsers = await _dbContext.Users
            .Where(x => x.IsSubmittedEmail == false)
            .Take(3)
            .ToListAsync();

        try
        {
            foreach (var user in unactivatedUsers)
            {
                var cachedValue = await _cache.GetStringAsync(user.Id.Value);

                if (cachedValue is null)
                {
                    _logger.LogWarning("Cant handle delete user {@Email}", user.Email.Value);
                    continue;
                }

                var dateUtc = JsonConvert.DeserializeObject<DateTime>(cachedValue);
                if (dateUtc < DateTime.UtcNow)
                {
                    await _cache.RemoveAsync(user.Id.Value);
                    _dbContext.Users.Remove(user);    
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Cant handle unactivated users delete");
        }
    }
}