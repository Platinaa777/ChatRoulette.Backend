using System.Collections.Concurrent;
using ProfileService.Domain.Models.Identity;
using ProfileService.Domain.Shared;

namespace ProfileService.Infrastructure.Repos.Interfaces;

public interface IChangeTracker
{
    ConcurrentBag<AggregateRoot<Id>> Entities { get; }

    public void Track(AggregateRoot<Id> entity);
}