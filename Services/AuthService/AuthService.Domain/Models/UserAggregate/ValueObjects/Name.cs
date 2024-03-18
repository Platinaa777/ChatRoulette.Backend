using AuthService.Domain.SeedWork;

namespace AuthService.Domain.Models.UserAggregate.ValueObjects;
 
public class Name : ValueObject
{
    public string Value { get; private set; }
    
    public Name(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("name can not be empty or null");
        
        Value = name;
    }

    public Name ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("name can not be empty or null");

        return new Name(name);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}