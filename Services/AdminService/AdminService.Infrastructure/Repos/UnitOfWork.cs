using AdminService.DataContext.Outbox;
using AdminService.Domain.Models.ComplaintAggregate.ValueObjects;
using AdminService.Domain.Models.Shared;
using DomainDriverDesignAbstractions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AdminService.Infrastructure.Repos;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext.Database.DataContext _dbContext;

    public UnitOfWork(DataContext.Database.DataContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [Obsolete("only for dapper implementation")]
    public ValueTask StartTransaction(CancellationToken token = default)
    {
        return ValueTask.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken token = default)
    {
        var domainEvents = _dbContext.ChangeTracker.Entries<AggregateRoot<Id>>()
            .Select(x => x.Entity)
            .SelectMany(root =>
            {
                var events = root.GetDomainEvents();
                root.ClearDomainEvents();
                return events;
            })
            .Select(domainEvent => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = domainEvent.GetType().Name,
                StartedAtUtc = DateTime.UtcNow,
                Content = JsonConvert.SerializeObject(domainEvent,
                    new JsonSerializerSettings()
                    {
                        TypeNameHandling = TypeNameHandling.All
                    })
            })
            .ToList();

        await _dbContext.OutboxMessages.AddRangeAsync(domainEvents, token);
        await _dbContext.SaveChangesAsync(token);
    }

    [Obsolete("only for dapper implementation")]
    public void Dispose() { }
}