using AuthService.DataContext.Database;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace AuthService.Api.BackgroundJobs;

[DisallowConcurrentExecution]
public class RefreshTokenCleanerJob : IJob
{
    private readonly ILogger<RefreshTokenCleanerJob> _logger;
    private readonly UserDb _dbContext;

    public RefreshTokenCleanerJob(
        ILogger<RefreshTokenCleanerJob> logger,
        UserDb dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            var affectedRows = await _dbContext
                .RefreshTokens
                .Where(token => token.IsUsed == true)
                .ExecuteDeleteAsync();

            _logger.LogInformation("{Count} tokens was deleted because was used in the past", affectedRows);

            await _dbContext.SaveChangesAsync(context.CancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error with deleting used refresh tokens was occured");
        }
    }
}