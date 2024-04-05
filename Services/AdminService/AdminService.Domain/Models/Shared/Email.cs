using System.Text.RegularExpressions;
using AdminService.Domain.Errors;
using DomainDriverDesignAbstractions;

namespace AdminService.Domain.Models.ComplaintAggregate.ValueObjects;

public class Email : ValueObject
{
    private static readonly Regex EmailValidator = new Regex("([a-zA-Z0-9._-]+@[a-zA-Z0-9._-]+\\.[a-zA-Z0-9_-]+)");
    
    public string Value { get; private set; }

    public static Result<Email> Create(string email)
    {
        if (!EmailValidator.IsMatch(email))
            return Result.Failure<Email>(ComplaintError.InvalidEmail);
        return new Email(email);
    }
    
    private Email(string email)
    {
        Value = email;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    private Email()
    {
        
    }
}