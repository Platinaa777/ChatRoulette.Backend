using DomainDriverDesignAbstractions;

namespace ProfileService.Domain.Models.UserProfileAggregate.ValueObjects;

public class Avatar : ValueObject
{
    public string Value { get; set; }

    public static Avatar Create(string avatar)
    {
        return new Avatar(avatar);
    }

    private Avatar(string avatar)
    {
        Value = avatar;
    }

    public bool IsExists()
    {
        return !string.IsNullOrWhiteSpace(Value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}