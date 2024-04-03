using DomainDriverDesignAbstractions;
using ProfileService.Domain.Models.UserProfileAggregate.Enumerations;
using ProfileService.Domain.Models.UserProfileAggregate.Errors;
using ProfileService.Domain.Models.UserProfileAggregate.ValueObjects;

namespace ProfileService.Domain.Models.UserProfileAggregate.Entities;

public class Achievement : Entity<int>
{

    public static Result<Achievement> Create(int id, string photo)
    {
        var achievementType = AchievementType.FromValue(id);
        if (achievementType is null)
            return Result.Failure<Achievement>(AchievementErrors.TypeDoesNotExist);
        
        var achievementContent = AchievementContent.FromValue(id);
        if (achievementContent is null)
            return Result.Failure<Achievement>(AchievementErrors.TypeDoesNotExist);

        var achievementBodyResult = AchievementBody.Create(
            title: achievementType.Name,
            content: achievementContent.Name,
            photo);
        
        if (achievementBodyResult.IsFailure)
            return Result.Failure<Achievement>(achievementBodyResult.Error);

        return new Achievement(
            id,
            achievementBodyResult.Value.Photo);
    }

    private Achievement(
        int id,
        string photo) : base(id)
    {
        Photo = photo;
    }

    public string Photo { get; private set; }
}