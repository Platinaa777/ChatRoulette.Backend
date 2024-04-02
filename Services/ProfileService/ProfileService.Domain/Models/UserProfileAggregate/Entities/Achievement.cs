using DomainDriverDesignAbstractions;
using ProfileService.Domain.Models.UserProfileAggregate.Enumerations;
using ProfileService.Domain.Models.UserProfileAggregate.Errors;
using ProfileService.Domain.Models.UserProfileAggregate.ValueObjects;

namespace ProfileService.Domain.Models.UserProfileAggregate.Entities;

public class Achievement : Entity<int>
{

    public static Result<Achievement> Create(int id, string content, string photo)
    {
        var achievementType = AchievementType.FromValue(id);
        if (achievementType is null)
            return Result.Failure<Achievement>(AchievementErrors.TypeDoesNotExist);

        var achievementBodyResult = AchievementBody.Create(achievementType.Name, content, photo);
        if (achievementBodyResult.IsFailure)
            return Result.Failure<Achievement>(achievementBodyResult.Error);

        return new Achievement(
            id,
            achievementBodyResult.Value.Content,
            achievementBodyResult.Value.Photo);
    }

    private Achievement(
        int id,
        string content,
        string photo) : base(id)
    {
        Content = content;
        Photo = photo;
    }

    public string Content { get; private set; }
    public string Photo { get; private set; }
}