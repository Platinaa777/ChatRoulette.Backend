using AuthService.Domain.SeedWork;

namespace AuthService.Domain.Models.UserAggregate.ValueObjects;

public class Age : ValueObject
{
    public int Value { get; private set; }

    public Age(int age)
    {
        if (age <= 0 || age >= 100)
            throw new ArgumentException($"Invalid age {age}");

        Value = age;
    }

    public Age Increase(int value)
    {
        if (value <= 0)
            throw new ArgumentException("Value can not be less than zero in age increasing");

        return new Age(Value + value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

}