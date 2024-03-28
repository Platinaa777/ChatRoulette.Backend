using ProfileService.Domain.Shared;

namespace ProfileService.Domain.Models.UserProfileAggregate.ValueObjects;

public class Rating : ValueObject
{
    private const ulong MAX_RATING = 1000;
    public ulong Value { get; set; }
    
    public static Result<Rating> Create(ulong rating)
    {
        if (MAX_RATING <= rating)
            return new Rating(MAX_RATING);
        return new Rating(rating);
    }
    
    private Rating(ulong rating)
    {
        Value = rating;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}