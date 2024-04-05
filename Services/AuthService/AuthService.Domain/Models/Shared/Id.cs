using AuthService.Domain.Errors.UserErrors;
using AuthService.Domain.Models.UserAggregate.ValueObjects;
using DomainDriverDesignAbstractions;

namespace AuthService.Domain.Models.Shared;

public class Id : ValueObject, IEquatable<Id>
{
    public string Value { get; private set; }

    private Id(string id)
    {
        Value = id;
    }

    public static Result<Id> CreateId(string id)
    {
        if (!Guid.TryParse(id, out var guidId))
            return Result.Failure<Id>(UserError.InvalidId);
        return new Id(guidId.ToString());
    }
        
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public bool Equals(Id? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return base.Equals(other) && Value == other.Value;
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