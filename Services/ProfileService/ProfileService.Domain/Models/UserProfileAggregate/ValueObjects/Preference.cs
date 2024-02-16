using ProfileService.Domain.Models.UserProfileAggregate.Enumerations;
using ProfileService.Domain.SeedWork;

namespace ProfileService.Domain.Models.UserProfileAggregate.ValueObjects;

public class Preference : ValueObject
{
    public Activity Activity { get; }
    public int PreferenceLevel { get; }
    
    public Preference(Activity activity, int preferenceLevel)
    {
        if (preferenceLevel < 0 || preferenceLevel > 100)
            throw new ArgumentException("Level should be in [0:100] segment");
        
        Activity = activity;
        PreferenceLevel = preferenceLevel;
    }

    public Preference ChangePreferenceLevel(int level)
    {
        if (level < 0 || level > 100)
            throw new ArgumentException("Level should be in [0:100] segment");

        return new Preference(Activity, level);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Activity;
        yield return PreferenceLevel;
    }
}