using AuthService.DataContext.Database;
using DomainDriverDesignAbstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;

namespace AuthService.Api.BackgroundJobs;

public class OutboxMessageJob : IJob
{
    private readonly UserDb _dbContext;
    private readonly IPublisher _publisher;
    private readonly ILogger<OutboxMessageJob> _logger;
    private static string WorkerName = "AuthService.OutboxMessageJob";

    public OutboxMessageJob(
        UserDb dbContext,
        IPublisher publisher,
        ILogger<OutboxMessageJob> logger)
    {
        _dbContext = dbContext;
        _publisher = publisher;
        _logger = logger;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        var outboxMessages = await _dbContext.OutboxMessages
            .AsTracking()
            .Where(x => x.HandledAtUtc == null)
            .Take(3)
            .ToListAsync();

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
                        nameof(OutboxMessageJob),
                        message.Id);
                    continue;
                }
                
                _logger.LogInformation(@"Outbox message {@BackgroundJobId} was received by {@Worker}, type: {@Type}, content: {@Content}",
                    message.Id.ToString(),
                    WorkerName,
                    message.Type,
                    message.Content);

                await _publisher.Publish(domainEvent);
                
                message.HandledAtUtc = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync();
                
                _logger.LogInformation("Outbox message {@BackgroundJobId} was handled by {@Worker}",
                    message.Id,
                    WorkerName);
            }
            catch 
            {
                _logger.LogError(@"Outbox message has failed {@Id}", message.Id);
            }
        }
    }
}