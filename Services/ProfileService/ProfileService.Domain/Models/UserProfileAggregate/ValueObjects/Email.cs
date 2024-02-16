using System.Text.RegularExpressions;
using ProfileService.Domain.SeedWork;

namespace ProfileService.Domain.Models.UserProfileAggregate.ValueObjects;

public class Email : ValueObject
{
    private static readonly Regex EmailValidator = new Regex("([a-zA-Z0-9._-]+@[a-zA-Z0-9._-]+\\.[a-zA-Z0-9_-]+)");
    
    //todo change to private set
    public string Value { get; private set; }
    public bool IsSubmitted { get; private set; }
    
    public Email(string email, bool isSubmitted = false)
    {
        if (!EmailValidator.IsMatch(email))
            throw new ArgumentException("Invalid email");

        Value = email;
        IsSubmitted = IsSubmitted;
    }

    public Email ChangeEmail(string email)
    {
        if (!EmailValidator.IsMatch(email))
            throw new ArgumentException("Invalid email");

        return new Email(email);
    }

    public Email SubmitEmail()
    {
        return new Email(Value, true);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return IsSubmitted.ToString();
        yield return Value;
    }
}