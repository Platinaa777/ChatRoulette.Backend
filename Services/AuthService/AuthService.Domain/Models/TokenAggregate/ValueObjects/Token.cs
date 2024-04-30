using AuthService.Domain.Errors;
using DomainDriverDesignAbstractions;

namespace AuthService.Domain.Models.TokenAggregate.ValueObjects;

public class Token : ValueObject
{
    public string Value { get; private set; }
    private Token(string token)
    {
        Value = token;
    }

    public static Result<Token> Create(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return Result.Failure<Token>(TokenError.EmptyToken);

        return new Token(token);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}