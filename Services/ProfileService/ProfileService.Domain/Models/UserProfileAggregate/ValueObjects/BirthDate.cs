using DomainDriverDesignAbstractions;
using ProfileService.Domain.Models.UserProfileAggregate.Errors;

namespace ProfileService.Domain.Models.UserProfileAggregate.ValueObjects;

public class BirthDateUtc : ValueObject
{
    public DateTime Value { get; private set; }
    
    public static Result<BirthDateUtc> Create(DateTime birthDayUtc)
    {
        birthDayUtc = birthDayUtc.ToUniversalTime();
        DateTime currentDateUtc = DateTime.UtcNow;
        DateTime sixteenYearsAgo = currentDateUtc.AddYears(-16);

        if (birthDayUtc > sixteenYearsAgo)
            return Result.Failure<BirthDateUtc>(UserProfileErrors.YoungUser);
        
        DateTime hundredYearsAgo = currentDateUtc.AddYears(-100);
        if (birthDayUtc < hundredYearsAgo)
            return Result.Failure<BirthDateUtc>(UserProfileErrors.OldUser);
        
        return new BirthDateUtc(birthDayUtc);
    }
    
    private BirthDateUtc(DateTime birthday)
    {
        Value = birthday;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}