using DomainDriverDesignAbstractions;
using ProfileService.Domain.Models.UserProfileAggregate.Errors;

namespace ProfileService.Domain.Models.UserProfileAggregate.ValueObjects;
 
public class Name : ValueObject
{
    public string Value { get; private set; }

    public static Result<Name> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<Name>(UserProfileErrors.EmptyName);
        return new Name(name);
    }
    
    private Name(string name)
    {
        Value = name;
    }

    public Result<Name> ChangeName(string name)
    {
        return Create(name);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    private Name() {}
}