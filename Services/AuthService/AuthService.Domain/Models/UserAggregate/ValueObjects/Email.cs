using System.Text.RegularExpressions;
using AuthService.Domain.Errors.UserErrors;
using AuthService.Domain.Shared;

namespace AuthService.Domain.Models.UserAggregate.ValueObjects;

public class Email : ValueObject
{
    private static readonly Regex EmailValidator = new Regex("([a-zA-Z0-9._-]+@[a-zA-Z0-9._-]+\\.[a-zA-Z0-9_-]+)");
    
    public string Value { get; private set; }

    public static Result<Email> Create(string email)
    {
        if (!EmailValidator.IsMatch(email))
            return Result.Failure<Email>(UserError.InvalidEmail);
        return new Email(email);
    }
    
    private Email(string email)
    {
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

    private Email()
    {
        
    }
}