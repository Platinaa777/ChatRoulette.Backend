using System.Transactions;
using Dapper;
using MediatR;
using Newtonsoft.Json;
using Npgsql;
using ProfileService.Domain.Shared;
using ProfileService.Infrastructure.OutboxPattern;
using ProfileService.Infrastructure.Repos.Interfaces;

namespace ProfileService.Infrastructure.Repos.Common;

public class UnitOfWork : IUnitOfWork
{
    private readonly IDbConnectionFactory<NpgsqlConnection> _factory;
    private readonly IChangeTracker _tracker;
    private readonly IPublisher _publisher;
    private NpgsqlTransaction? _transaction;

    public UnitOfWork(
        IDbConnectionFactory<NpgsqlConnection> factory, 
        IChangeTracker tracker,
        IPublisher publisher)
    {
        _factory = factory;
        _tracker = tracker;
        _publisher = publisher;
    }

    public async ValueTask StartTransaction(CancellationToken token)
    {
        if (_transaction != null) return;
        
        var connection = await _factory.CreateConnection(token);
        _transaction = await connection.BeginTransactionAsync(token);
    }

    public async Task SaveChangesAsync(CancellationToken token)
    {
        if (_transaction == null)
            throw new TransactionException("transaction is not started");
        
        var domainEvents = new List<OutboxMessage>(
            _tracker.Entities.SelectMany(events =>
                {
                    var collectionEvents = events.GetDomainEvents();
                    events.ClearDomainEvents();
                    return collectionEvents;
                }).Select(domainEvent => new OutboxMessage()
                    {
                        Id = Guid.NewGuid(),
                        Type = domainEvent.GetType().Name,
                        StartedAtUtc = DateTime.UtcNow,
                        Content = JsonConvert.SerializeObject(domainEvent, 
                            new JsonSerializerSettings() {TypeNameHandling = TypeNameHandling.All})
                    })).ToList();
        
        var connection = await _factory.CreateConnection(token);
        
        foreach (var outboxMessage in domainEvents)
        {
            await connection.ExecuteAsync(@"
                    INSERT INTO outbox_messages (id, type, content, started_at, handled_at, error)
                    VALUES 
                    (@Id, @Type, @Content, @StartedAtUtc, @HandledAtUtc, @Error);", outboxMessage);
        }
        
        await _transaction.CommitAsync(token);
    }

    public void Dispose()
    {
        _transaction?.Dispose();
    }
}