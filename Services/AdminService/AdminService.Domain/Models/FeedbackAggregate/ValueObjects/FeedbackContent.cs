using AdminService.Domain.Errors;
using DomainDriverDesignAbstractions;

namespace AdminService.Domain.Models.FeedbackAggregate.ValueObjects;

public class FeedbackContent : ValueObject
{
    public string Value { get; private set; }

    public static Result<FeedbackContent> Create(string content)
    {
        if (content.Trim().Length < 10)
            return Result.Failure<FeedbackContent>(FeedbackError.NotEnoughLength);

        return new FeedbackContent(content);
    }
    
    private FeedbackContent(string content)
    {
        Value = content;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}