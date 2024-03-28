namespace AuthService.Domain.Shared;

public class AggregateRoot : Entity<string>
{
    private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
    
    protected AggregateRoot(string id) : base(id)
    {
    }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public List<IDomainEvent> GetDomainEvents() =>
        _domainEvents;

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
    
    protected AggregateRoot() {}
}