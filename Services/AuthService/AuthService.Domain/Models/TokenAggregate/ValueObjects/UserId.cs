using AuthService.Domain.Errors.TokenErrors;
using AuthService.Domain.Shared;

namespace AuthService.Domain.Models.TokenAggregate.ValueObjects;

public class UserId : ValueObject
{
    public string Value { get; set; }

    private UserId(string id)
    {
        Value = id;
    }

    public static Result<UserId> CreateId(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return Result.Failure<UserId>(TokenError.EmptyUserId);
        return new UserId(id);
    }
        
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}