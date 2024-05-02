using Chat.DataContext.Database;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace Chat.Api.BackgroundJobs;

[DisallowConcurrentExecution]
public class ChatUserHistoryCleanerJob : IJob
{
    private readonly ILogger<ChatUserHistoryCleanerJob> _logger;
    private readonly ChatDbContext _dbContext;

    public ChatUserHistoryCleanerJob(
        ILogger<ChatUserHistoryCleanerJob> logger,
        ChatDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            var affectedRows = await _dbContext.ChatUsers
                .AsQueryable()
                .Take(20)
                .ExecuteUpdateAsync(x =>
                      x.SetProperty(a => a.PreviousParticipantEmails, new HashSet<string>()));

            await _dbContext.SaveChangesAsync(context.CancellationToken);
            
            _logger.LogInformation("Clear chat history for {@Count} chat users", affectedRows);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error with cleaning chat user history was occured");
        }
    }
}