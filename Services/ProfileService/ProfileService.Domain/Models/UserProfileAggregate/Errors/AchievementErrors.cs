using DomainDriverDesignAbstractions;

namespace ProfileService.Domain.Models.UserProfileAggregate.Errors;

public class AchievementErrors : Error
{
    public static readonly Error EmptyTitle = new("UserProfile.Achievement.Error", "Title of achievement cant be empty");
    public static readonly Error EmptyContent = new("UserProfile.Achievement.Error", "Content of achievement cant be empty");
    public static readonly Error TypeDoesNotExist = new("UserProfile.Achievement.Error", "This type of achievement does not exist");

    
    public AchievementErrors(string code, string message) : base(code, message)
    {
    }
}