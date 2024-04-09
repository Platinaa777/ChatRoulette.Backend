using AuthService.Domain.Errors.UserErrors;
using DomainDriverDesignAbstractions;

namespace AuthService.Domain.Models.UserAggregate.ValueObjects;

public class Age : ValueObject
{
    public int Value { get; private set; }

    public static Result<Age> Create(int age)
    {
        if (age <= 15 || age >= 100)
            return Result.Failure<Age>(UserError.InvalidAge);
        return new Age(age);
    }
    private Age(int age)
    {
        Value = age;
    }

    public Result<Age> Increase(int value)
    {
        if (value <= 0)
            return Result.Failure<Age>(UserError.InvalidAgeIncrease);

        return new Age(Value + value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

}