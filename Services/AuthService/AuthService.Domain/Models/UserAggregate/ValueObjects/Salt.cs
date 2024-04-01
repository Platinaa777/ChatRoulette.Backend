using DomainDriverDesignAbstractions;

namespace AuthService.Domain.Models.UserAggregate.ValueObjects;

public class Salt : ValueObject
{
    public string Value { get; private set; }
    
    public Salt(string salt)
    {
        Value = salt;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}