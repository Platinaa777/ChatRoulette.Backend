using System.Collections.Concurrent;
using ProfileService.Domain.Models.Identity;
using ProfileService.Domain.Shared;
using ProfileService.Infrastructure.Repos.Interfaces;

namespace ProfileService.Infrastructure.Repos.Implementations;

public class ChangeTracker : IChangeTracker
{
    public ConcurrentBag<AggregateRoot<Id>> Entities { get; }

    public ChangeTracker()
    {
        Entities = new ConcurrentBag<AggregateRoot<Id>>();
    }
    
    public void Track(AggregateRoot<Id> entity)
    {
        Entities.Add(entity);
    }
}