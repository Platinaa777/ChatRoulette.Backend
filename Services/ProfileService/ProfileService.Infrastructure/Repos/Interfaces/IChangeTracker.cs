using System.Collections.Concurrent;
using ProfileService.Domain.SeedWork;

namespace ProfileService.Infrastructure.Repos.Interfaces;

public interface IChangeTracker
{
    ConcurrentBag<Entity<string>> Entities { get; }

    public void Track(Entity<string> entity);
}