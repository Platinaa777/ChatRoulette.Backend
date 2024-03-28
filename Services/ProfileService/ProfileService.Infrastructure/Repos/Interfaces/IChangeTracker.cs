using System.Collections.Concurrent;
using ProfileService.Domain.Shared;

namespace ProfileService.Infrastructure.Repos.Interfaces;

public interface IChangeTracker
{
    ConcurrentBag<AggregateRoot<Guid>> Entities { get; }

    public void Track(AggregateRoot<Guid> entity);
}