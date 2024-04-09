using Dapper;
using DomainDriverDesignAbstractions;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Npgsql;
using ProfileService.Infrastructure.Configuration;
using ProfileService.Infrastructure.OutboxPattern;
using ProfileService.Infrastructure.Repos.Interfaces;
using Quartz;
using Serilog;

namespace ProfileService.Api.BackgroundJobs;

[DisallowConcurrentExecution]
public class OutboxBackgroundJob : IJob
{
    private readonly IPublisher _publisher;
    private readonly ILogger<OutboxBackgroundJob> _logger;
    private readonly DatabaseOptions _connectionString;
    private string WorkerName = "ProfileService.OutboxBackgroundJob";

    public OutboxBackgroundJob(
        IPublisher publisher,
        ILogger<OutboxBackgroundJob> logger,
        IOptions<DatabaseOptions> connectionString)
    {
        _publisher = publisher;
        _logger = logger;
        _connectionString = connectionString.Value;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var connection = new NpgsqlConnection(_connectionString.ConnectionString);
        await connection.OpenAsync(context.CancellationToken);

        var outboxMessages = await connection
            .QueryAsync<OutboxMessage>(
                @"SELECT * FROM outbox_messages
                     WHERE handled_at is null
                     LIMIT 10");

        foreach (var message in outboxMessages)
        {
            try
            {
                var transaction = await connection.BeginTransactionAsync(context.CancellationToken);
                
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

                await transaction.CommitAsync(context.CancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(@"Outbox message has failed {@Id} with error message {@ErrorMessage}",
                    message.Id,
                    e.Message);
            }
        }
        
        await connection.DisposeAsync();
    }
}