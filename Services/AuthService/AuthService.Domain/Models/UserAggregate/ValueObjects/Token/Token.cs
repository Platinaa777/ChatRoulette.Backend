using AuthService.Domain.SeedWork;

namespace AuthService.Domain.Models.UserAggregate.ValueObjects.Token;

public class Token : ValueObject
{
    public string Value { get; set; }
    public Token(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("token cant be empty or null");

        Value = token;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}