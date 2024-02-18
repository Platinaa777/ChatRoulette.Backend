using System.Collections.Concurrent;
using ProfileService.Domain.SeedWork;
using ProfileService.Infrastructure.Repos.Interfaces;

namespace ProfileService.Infrastructure.Repos.Implementations;

public class ChangeTracker : IChangeTracker
{
    public ConcurrentBag<Entity<string>> Entities { get; }

    public ChangeTracker()
    {
        Entities = new ConcurrentBag<Entity<string>>();
    }
    
    public void Track(Entity<string> entity)
    {
        Entities.Add(entity);
    }
}