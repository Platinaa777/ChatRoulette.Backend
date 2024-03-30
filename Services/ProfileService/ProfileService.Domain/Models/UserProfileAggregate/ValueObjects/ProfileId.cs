using ProfileService.Domain.Models.Identity;
using ProfileService.Domain.Models.UserProfileAggregate.Errors;
using ProfileService.Domain.Shared;

namespace ProfileService.Domain.Models.UserProfileAggregate.ValueObjects;

public class ProfileId
    : ValueObject, IEquatable<ProfileId>
        
{
    public Guid Value { get; set; }
    
    public static Result<ProfileId> Create(string id)
    {
        if (Guid.TryParse(id, out var result))
        {
            return new ProfileId(result);
        }

        return Result.Failure<ProfileId>(UserProfileErrors.InvalidProfileId);
    }
    
    private ProfileId(Guid id)
    {
        Value = id;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public bool Equals(ProfileId? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return base.Equals(other) && Value == other.Value;
    }

    public bool Equals(Id? other)
    {
        throw new NotImplementedException();
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((ProfileId)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Value);
    }
}