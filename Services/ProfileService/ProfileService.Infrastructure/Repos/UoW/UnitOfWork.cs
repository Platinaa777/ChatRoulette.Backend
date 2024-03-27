using System.Transactions;
using MediatR;
using Npgsql;
using ProfileService.Domain.Shared;
using ProfileService.Infrastructure.Repos.ConnectionFactories;
using ProfileService.Infrastructure.Repos.Interfaces;

namespace ProfileService.Infrastructure.Repos.UoW;

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
        if (_transaction == null) throw new TransactionException("transaction is not started");
        
        var domainEvents = new Queue<INotification>(_tracker.Entities
                .SelectMany(events =>
                    {
                        var collectionEvents = events.DomainEvents;
                        events.DomainEvents.Clear();
                        return collectionEvents;
                    }));

        while (domainEvents.TryDequeue(out var notification))
        {
            await _publisher.Publish(notification, token);
        }
        
        await _transaction.CommitAsync(token);
    }

    public void Dispose()
    {
        _transaction?.Dispose();
    }
}