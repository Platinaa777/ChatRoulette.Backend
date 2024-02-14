using AuthService.Domain.SeedWork;

namespace AuthService.Domain.Models.UserAggregate.ValueObjects;

public class Password : ValueObject
{
    public string Value { get; set; }

    public Password(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length <= 5)
            throw new ArgumentException($"Invalid password {password}");

        Value = password;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}