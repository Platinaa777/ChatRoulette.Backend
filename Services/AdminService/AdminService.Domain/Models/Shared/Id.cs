using AdminService.Domain.Errors;
using DomainDriverDesignAbstractions;

namespace AdminService.Domain.Models.Shared;

public class Id : ValueObject, IEquatable<Id>
{
    public Guid Value { get; init; }

    public static Result<Id> Create(string id)
    {
        if (!Guid.TryParse(id, out var guidId))
        {
            return Result.Failure<Id>(ComplaintError.InvalidId);
        }

        return new Id(guidId);
    }

    private Id(Guid id)
    {
        Value = id;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

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

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Value);
    }
}