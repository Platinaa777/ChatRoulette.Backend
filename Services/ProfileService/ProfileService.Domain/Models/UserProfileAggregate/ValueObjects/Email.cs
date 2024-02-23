using System.Text.RegularExpressions;
using ProfileService.Domain.SeedWork;

namespace ProfileService.Domain.Models.UserProfileAggregate.ValueObjects;

public class Email : ValueObject
{
    private static readonly Regex EmailValidator = new Regex("([a-zA-Z0-9._-]+@[a-zA-Z0-9._-]+\\.[a-zA-Z0-9_-]+)");
    
    public string Value { get; private set; }
    
    public Email(string email)
    {
        if (!EmailValidator.IsMatch(email))
            throw new ArgumentException("Invalid email");

        Value = email;
    }

    public Email ChangeEmail(string email)
    {
        if (!EmailValidator.IsMatch(email))
            throw new ArgumentException("Invalid email");

        return new Email(email);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}