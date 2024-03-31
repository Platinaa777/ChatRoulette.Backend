using Dapper;
using MediatR;
using Newtonsoft.Json;
using Npgsql;
using ProfileService.Domain.Shared;
using ProfileService.Infrastructure.OutboxPattern;
using ProfileService.Infrastructure.Repos.Interfaces;
using Quartz;

namespace ProfileService.Api.BackgroundJobs;

[DisallowConcurrentExecution]
public class OutboxBackgroundJob : IJob
{
    private readonly IPublisher _publisher;
    private readonly IDbConnectionFactory<NpgsqlConnection> _connectionFactory;
    private readonly ILogger<OutboxBackgroundJob> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public OutboxBackgroundJob(
        IPublisher publisher,
        IDbConnectionFactory<NpgsqlConnection> connectionFactory,
        ILogger<OutboxBackgroundJob> logger,
        IUnitOfWork unitOfWork)
    {
        _publisher = publisher;
        _connectionFactory = connectionFactory;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _unitOfWork.StartTransaction(default);
        
        var connection = await _connectionFactory.CreateConnection(default);

        var outboxMessages = await connection
            .QueryAsync<OutboxMessage>(
                @"SELECT * FROM outbox_messages
                     WHERE handled_at is null
                     LIMIT 10");

        foreach (var message in outboxMessages)
        {
            try
            {
                IDomainEvent? domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(message.Content);
                if (domainEvent is null)
                {
                    _logger.LogWarning(@"Should debug background job: {@job} with domain event: {@event}",
                        nameof(OutboxBackgroundJob),
                        message.Id);
                    continue;
                }

                await _publisher.Publish(domainEvent);
                
                message.HandledAtUtc = DateTime.UtcNow;
                
            }
            catch (Exception e)
            {
                _logger.LogError(@"Outbox message has failed {@Id}", message.Id);
            }
        }

        await _unitOfWork.SaveChangesAsync(default);
    }
}