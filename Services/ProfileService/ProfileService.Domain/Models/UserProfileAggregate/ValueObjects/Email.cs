using System.Text.RegularExpressions;
using DomainDriverDesignAbstractions;
using ProfileService.Domain.Models.UserProfileAggregate.Errors;

namespace ProfileService.Domain.Models.UserProfileAggregate.ValueObjects;

public class Email : ValueObject
{
    private static readonly Regex EmailValidator = new Regex("([a-zA-Z0-9._-]+@[a-zA-Z0-9._-]+\\.[a-zA-Z0-9_-]+)");
    
    public string Value { get; private set; }

    public static Result<Email> Create(string email)
    {
        if (!EmailValidator.IsMatch(email))
            return Result.Failure<Email>(UserProfileErrors.InvalidEmail);
        return new Email(email);
    }
    
    private Email(string email)
    {
        Value = email;
    }

    public Result<Email> ChangeEmail(string email)
    {
        if (!EmailValidator.IsMatch(email))
            return Result.Failure<Email>(UserProfileErrors.InvalidEmail);

        return new Email(email);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    private Email() {}
}