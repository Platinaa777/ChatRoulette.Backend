using MediatR;

namespace AuthService.Domain.SeedWork;

public abstract class Entity<TKey> : IEquatable<Entity<string>> 
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

    public override int GetHashCode() =>
        Id.GetHashCode() * 137;
    

    public bool Equals(Entity<string>? other) =>
        other is not null && Id.Equals(other.Id);

    
    protected Entity() {}
}