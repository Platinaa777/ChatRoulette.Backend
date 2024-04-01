namespace DomainDriverDesignAbstractions;

public abstract class Entity<TKey> : IEquatable<Entity<TKey>> 
    where TKey : IEquatable<TKey>
{
    public TKey Id { get; init; }
    protected Entity(TKey id) => Id = id;


    public override bool Equals(object? other)
    {
        if (other is null) return false;
        if (other.GetType() != GetType()) return false;
        
        return other is Entity<TKey> obj && Equals(obj);
    }

    public override int GetHashCode()
    {
        return EqualityComparer<TKey>.Default.GetHashCode(Id);
    }


    public bool Equals(Entity<string>? other) =>
        other is not null && Id.Equals(other.Id);

    
    protected Entity() {}

    public bool Equals(Entity<TKey>? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return EqualityComparer<TKey>.Default.Equals(Id, other.Id);
    }
}