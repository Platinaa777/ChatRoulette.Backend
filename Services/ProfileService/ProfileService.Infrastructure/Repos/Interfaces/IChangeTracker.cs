using System.Collections.Concurrent;
using DomainDriverDesignAbstractions;
using ProfileService.Domain.Models.Identity;

namespace ProfileService.Infrastructure.Repos.Interfaces;

public interface IChangeTracker
{
    ConcurrentBag<AggregateRoot<Id>> Entities { get; }

    public void Track(AggregateRoot<Id> entity);
}