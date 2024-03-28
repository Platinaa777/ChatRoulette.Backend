using System.Collections.Concurrent;
using ProfileService.Domain.Shared;
using ProfileService.Infrastructure.Repos.Interfaces;

namespace ProfileService.Infrastructure.Repos.Implementations;

public class ChangeTracker : IChangeTracker
{
    public ConcurrentBag<AggregateRoot<Guid>> Entities { get; }

    public ChangeTracker()
    {
        Entities = new ConcurrentBag<AggregateRoot<Guid>>();
    }
    
    public void Track(AggregateRoot<Guid> entity)
    {
        Entities.Add(entity);
    }
}