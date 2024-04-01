using AuthService.Domain.Errors.UserErrors;
using DomainDriverDesignAbstractions;

namespace AuthService.Domain.Models.UserAggregate.ValueObjects;

public class Password : ValueObject
{
    public string Value { get; private set; }

    public static Result<Password> Create(string password)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length <= 5)
            return Result.Failure<Password>(UserError.SmallPassword);
        return new Password(password);
    }
    private Password(string password)
    {
        Value = password;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}