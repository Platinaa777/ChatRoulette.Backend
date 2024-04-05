using System.Text.Json.Serialization;
using AuthService.DataContext.Database;
using AuthService.DataContext.Outbox;
using AuthService.Domain.Models.Shared;
using DomainDriverDesignAbstractions;
using Newtonsoft.Json;

namespace AuthService.Infrastructure.Repos;

public class UnitOfWork : IUnitOfWork
{
    private readonly UserDb _context;

    public UnitOfWork(UserDb context)
    {
        _context = context;
    }

    public ValueTask StartTransaction(CancellationToken token = default)
    {
        return ValueTask.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = _context.ChangeTracker
            .Entries<AggregateRoot<Id>>()
            .Select(x => x.Entity)
            .SelectMany(root =>
            {
                var events = root.GetDomainEvents();
                root.ClearDomainEvents();
                return events;
            }).Select(domainEvent => new OutboxMessage()
            {
                Id = Guid.NewGuid(),
                Type = domainEvent.GetType().Name,
                StartedAtUtc = DateTime.UtcNow,
                Content = JsonConvert.SerializeObject(
                    domainEvent,
                    new JsonSerializerSettings()
                    {
                        TypeNameHandling = TypeNameHandling.All
                    })
            }).ToList();

        await _context.OutboxMessages.AddRangeAsync(domainEvents, cancellationToken);
            
        await _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose() { }
}