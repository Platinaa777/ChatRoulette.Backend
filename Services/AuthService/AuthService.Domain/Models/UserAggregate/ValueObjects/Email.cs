using System.Text.RegularExpressions;
using AuthService.Domain.SeedWork;

namespace AuthService.Domain.Models.UserAggregate.ValueObjects;

public class Email : ValueObject
{
    private readonly Regex _emailValidator = new Regex("([a-zA-Z0-9._-]+@[a-zA-Z0-9._-]+\\.[a-zA-Z0-9_-]+)");

    public string Value { get; set; }
    public bool IsSubmitted { get; set; }
    
    public Email(string email, bool isSubmitted = false)
    {
        if (!_emailValidator.IsMatch(email))
            throw new ArgumentException("Invalid email");

        Value = email;
        IsSubmitted = IsSubmitted;
    }

    public Email ChangeEmail(string email)
    {
        if (!_emailValidator.IsMatch(email))
            throw new ArgumentException("Invalid email");

        return new Email(email);
    }

    public Email SubmitEmail()
    {
        return new Email(Value, true);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public Email()
    {
        
    }
}