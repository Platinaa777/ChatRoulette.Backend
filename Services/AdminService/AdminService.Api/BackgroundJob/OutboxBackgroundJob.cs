using DomainDriverDesignAbstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;

namespace AdminService.Api.BackgroundJob;

[DisallowConcurrentExecution]
public class OutboxBackgroundJob : IJob
{
    private readonly DataContext.Database.DataContext _dbContext;
    private readonly ILogger<OutboxBackgroundJob> _logger;
    private readonly IPublisher _publisher;

    public OutboxBackgroundJob(
        DataContext.Database.DataContext dbContext,
        ILogger<OutboxBackgroundJob> logger,
        IPublisher publisher)
    {
        _dbContext = dbContext;
        _logger = logger;
        _publisher = publisher;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        var outboxMessages = await _dbContext.OutboxMessages
            .AsTracking()
            .Where(x => x.HandledAtUtc == null)
            .Take(3)
            .ToListAsync();

        _logger.LogInformation("outbox message count: " + outboxMessages.Count());
        foreach (var message in outboxMessages)
        {
            try
            {
                IDomainEvent? domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(message.Content,
                    new JsonSerializerSettings()
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
                if (domainEvent is null)
                {
                    _logger.LogWarning(@"Should debug background job: {@job} with domain event: {@event}",
                        nameof(OutboxBackgroundJob),
                        message.Id);
                    continue;
                }

                await _publisher.Publish(domainEvent);
                
                message.HandledAtUtc = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, @"Outbox message has failed {@Id}", message.Id);
            }
        }
    }
}