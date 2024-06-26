namespace DomainDriverDesignAbstractions;

public class AggregateRoot<T> : Entity<T> where T : IEquatable<T>
{
    private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
    
    protected AggregateRoot(T id) : base(id)
    {
    }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public List<IDomainEvent> GetDomainEvents() =>
        _domainEvents.ToList();

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
    
    protected AggregateRoot() {}
}