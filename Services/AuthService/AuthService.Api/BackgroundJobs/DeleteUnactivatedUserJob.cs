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
            _logger.LogInformation("Minimum Count of unactivated users {@Count}", unactivatedUsers.Count);
            foreach (var user in unactivatedUsers)
            {
                var cachedValue = await _cache.GetStringAsync(user.Id.Value);

                if (cachedValue is null)
                {
                    _logger.LogWarning("User with email: {@Email} was deleted by {@Job}", user.Email.Value, nameof(DeleteUnactivatedUserJob));
                    _dbContext.Users.Remove(user);
                    await _dbContext.SaveChangesAsync(context.CancellationToken);
                    continue;
                }

                var dateUtc = JsonConvert.DeserializeObject<DateTime>(cachedValue);
                if (dateUtc < DateTime.UtcNow)
                {
                    _logger.LogInformation("User with id: {@Id} was deleted by {@Job}", user.Id.Value, nameof(DeleteUnactivatedUserJob));
                    await _cache.RemoveAsync(user.Id.Value);
                    _dbContext.Users.Remove(user);
                    await _dbContext.SaveChangesAsync(context.CancellationToken);
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Cant handle unactivated users delete");
        }
    }
}