using DomainDriverDesignAbstractions;

namespace ProfileService.Domain.Models.UserProfileAggregate.Enumerations;

public class AchievementContent : Enumeration
{
    public static readonly AchievementContent ManyFacesOfProfile = new AchievementContent(1, "Change avatar more than 5 times");
    public static readonly AchievementContent ManyFriends = new AchievementContent(2, "Get 35 friends");
    public static readonly AchievementContent DoomSlayer = new AchievementContent(3, "Ban 35 players");
    public static readonly AchievementContent MasterOfAdvancement = new AchievementContent(4, "Get 250 level");

    public AchievementContent(int id, string name) : base(id, name)
    {
    }
    
    public static AchievementContent? FromName(string name)
    {
        var collection = GetAll<AchievementContent>();
        foreach (var achievement in collection)
        {
            if (achievement.Name == name)
                return achievement;
        }

        return null;
    }
    
    public static AchievementContent? FromValue(int id)
    {
        var collection = GetAll<AchievementContent>();
        foreach (var achievement in collection)
        {
            if (achievement.Id == id)
                return achievement;
        }

        return null;
    }
}