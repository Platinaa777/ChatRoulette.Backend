using DomainDriverDesignAbstractions;
using ProfileService.Domain.Models.UserProfileAggregate.Errors;

namespace ProfileService.Domain.Models.UserProfileAggregate.ValueObjects;

public class Rating : ValueObject
{
    private const int MAX_RATING = 1000;
    public int Value { get; set; }
    
    public static Result<Rating> Create(int rating)
    {
        if (rating < 0)
            return Result.Failure<Rating>(UserProfileErrors.RatingShouldBePositive);
        if (MAX_RATING <= rating)
            return new Rating(MAX_RATING);
        return new Rating(rating);
    }
    
    private Rating(int rating)
    {
        Value = rating;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}