using DomainDriverDesignAbstractions;

namespace ProfileService.Domain.Models.Identity;

public class Id : ValueObject, IEquatable<Id>
{
    public Guid Value { get; }

    public static Result<Id> Create(string id)
    {
        if (Guid.TryParse(id, out var guidId))
        {
            return new Id(guidId);
        }

        return Result.Failure<Id>(InvalidId);
    }
    
    private Id(Guid id)
    {
        Value = id;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    public static readonly Error InvalidId = new("Identity.Error", "Invalid id");

    public bool Equals(Id? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return base.Equals(other) && Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Id)obj);
    }

    public static bool operator==(Id? left, Id? right)
        => left is not null && right is not null && left.Equals(right);

    public static bool operator!=(Id? left, Id? right) => !(left == right);

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Value);
    }
}