using ProfileService.Domain.SeedWork;

namespace ProfileService.Domain.Models.UserProfileAggregate.Enumerations;

public class Activity : Enumeration
{
    public static Activity Sport = new Activity(1, nameof(Sport));
    public static Activity ComputerScience = new Activity(1, nameof(ComputerScience));
    public static Activity Art = new Activity(1, nameof(Art));

    protected Activity(int id, string name)
    {
        
    }
    
    
    public static Activity? FromName(string name)
    {
        var collection = GetAll<Activity>();
        foreach (var activity in collection)
        {
            if (activity.Name == name)
                return activity;
        }

        return null;
    }
    
    public static Activity? FromValue(int id)
    {
        var collection = GetAll<Activity>();
        foreach (var activity in collection)
        {
            if (activity.Id == id)
                return activity;
        }

        return null;
    }

}