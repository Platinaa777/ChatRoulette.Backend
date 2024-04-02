using DomainDriverDesignAbstractions;
using ProfileService.Domain.Models.UserProfileAggregate.Errors;

namespace ProfileService.Domain.Models.UserProfileAggregate.ValueObjects;

public class AchievementBody : ValueObject
{
    public static Result<AchievementBody> Create(string title, string content, string photo)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Result.Failure<AchievementBody>(AchievementErrors.EmptyTitle);
        
        if (string.IsNullOrWhiteSpace(content))
            return Result.Failure<AchievementBody>(AchievementErrors.EmptyContent);

        return new AchievementBody(title, content, photo);
    }
    
    private AchievementBody(string title, string content, string photo)
    {
        Title = title;
        Content = content;
        Photo = photo;
    }
    
    public string Title { get; private set; }
    public string Content { get; private set; }
    public string Photo { get; private set; }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Title;
        yield return Content;
        yield return Photo;
    }
}