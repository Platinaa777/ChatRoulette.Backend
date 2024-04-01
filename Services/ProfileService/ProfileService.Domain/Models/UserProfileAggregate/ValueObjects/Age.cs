using DomainDriverDesignAbstractions;
using ProfileService.Domain.Models.UserProfileAggregate.Errors;

namespace ProfileService.Domain.Models.UserProfileAggregate.ValueObjects;

public class Age : ValueObject
{
    public int Value { get; private set; }

    public static Result<Age> Create(int age)
    {
        if (age <= 0 || age >= 100)
            return Result.Failure<Age>(UserProfileErrors.InvalidAge);
        return new Age(age);
    }
    private Age(int age)
    {
        Value = age;
    }

    public Result<Age> Increase(int value)
    {
        if (value <= 0)
            return Result.Failure<Age>(UserProfileErrors.InvalidAgeIncrease);

        return new Age(Value + value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}