using MediatR;

namespace Chat.Domain.SeedWork;

public abstract class Entity : IEquatable<Entity>
{
    public string Id { get; protected init; }

    public bool Equals(Entity? other)
    {
        if (other is null) return false;
        if (other.GetType() != GetType()) return false;

        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (obj.GetType() != GetType()) return false;
        if (obj is not Entity entity) return false;
        
        return entity.Id.Equals(Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode() * 137;
    }
    public static bool operator ==(Entity? left, Entity? right)
    {
        return left is not null && right is not null && left.Equals(left);
    }
    public static bool operator !=(Entity left, Entity right)
    {
        return !(left == right);
    }
}