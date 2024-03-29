using ProfileService.Domain.Shared;

namespace ProfileService.Domain.Models.UserProfileAggregate.Enumerations;

public class Preference : Enumeration
{
    public static readonly Preference Sport = new Preference(1, nameof(Sport));
    public static readonly Preference ComputerScience = new Preference(2, nameof(ComputerScience));
    public static readonly Preference Art = new Preference(3, nameof(Art));

    private Preference(int id, string name) : base(id, name) { }
    
    public static Preference? FromName(string name)
    {
        var collection = GetAll<Preference>();
        foreach (var activity in collection)
        {
            if (activity.Name == name)
                return activity;
        }

        return null;
    }
    
    public static Preference? FromValue(int id)
    {
        var collection = GetAll<Preference>();
        foreach (var activity in collection)
        {
            if (activity.Id == id)
                return activity;
        }

        return null;
    }
}