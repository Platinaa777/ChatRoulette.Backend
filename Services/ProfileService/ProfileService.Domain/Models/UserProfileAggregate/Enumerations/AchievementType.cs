using DomainDriverDesignAbstractions;

namespace ProfileService.Domain.Models.UserProfileAggregate.Enumerations;

public class AchievementType : Enumeration
{
    public static readonly AchievementType NothingToDo = new AchievementType(1, nameof(NothingToDo));
    public static readonly AchievementType ManyFriends = new AchievementType(2, nameof(ManyFriends));
    public static readonly AchievementType DoomSlayer = new AchievementType(3, nameof(DoomSlayer));
    public static readonly AchievementType ShapeShifter = new AchievementType(4, nameof(ShapeShifter));

    public AchievementType(int id, string name) : base(id, name)
    {
    }
    
    public static AchievementType? FromName(string name)
    {
        var collection = GetAll<AchievementType>();
        foreach (var achievement in collection)
        {
            if (achievement.Name == name)
                return achievement;
        }

        return null;
    }
    
    public static AchievementType? FromValue(int id)
    {
        var collection = GetAll<AchievementType>();
        foreach (var achievement in collection)
        {
            if (achievement.Id == id)
                return achievement;
        }

        return null;
    }
}