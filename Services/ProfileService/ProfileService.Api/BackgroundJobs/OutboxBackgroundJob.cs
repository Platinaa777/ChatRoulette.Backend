using Dapper;
using DomainDriverDesignAbstractions;
using MediatR;
using Newtonsoft.Json;
using Npgsql;
using ProfileService.Infrastructure.OutboxPattern;
using ProfileService.Infrastructure.Repos.Interfaces;
using Quartz;
using Serilog;

namespace ProfileService.Api.BackgroundJobs;

[DisallowConcurrentExecution]
public class OutboxBackgroundJob : IJob
{
    private readonly IPublisher _publisher;
    private readonly IDbConnectionFactory<NpgsqlConnection> _connectionFactory;
    private readonly ILogger<OutboxBackgroundJob> _logger;
    private string WorkerName = "ProfileService.OutboxBackgroundJob";

    public OutboxBackgroundJob(
        IPublisher publisher,
        IDbConnectionFactory<NpgsqlConnection> connectionFactory,
        ILogger<OutboxBackgroundJob> logger)
    {
        _publisher = publisher;
        _connectionFactory = connectionFactory;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
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
                
                _logger.LogInformation(@"Outbox message {@BackgroundJobId} was received by {@Worker}, type: {@Type}, content: {@Content}",
                    message.Id.ToString(),
                    WorkerName,
                    message.Type,
                    message.Content);

                await _publisher.Publish(domainEvent);
                
                message.HandledAtUtc = DateTime.UtcNow;

                var parameter = new
                {
                    Id = message.Id,
                    HandledAt = message.HandledAtUtc
                };

                await connection.ExecuteAsync(@"
                    UPDATE outbox_messages
                    SET handled_at = @HandledAt
                    WHERE id = @Id;                        
                ", parameter);
                
                _logger.LogInformation("Outbox message {@BackgroundJobId} was handled by {@Worker}",
                    message.Id,
                    WorkerName);
            }
            catch (Exception e)
            {
                _logger.LogError(@"Outbox message has failed {@Id}", message.Id);
            }
        }
    }
}