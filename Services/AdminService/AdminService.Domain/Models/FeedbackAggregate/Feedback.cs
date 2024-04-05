using AdminService.Domain.Models.ComplaintAggregate.ValueObjects;
using AdminService.Domain.Models.FeedbackAggregate.ValueObjects;
using AdminService.Domain.Models.Shared;
using DomainDriverDesignAbstractions;

namespace AdminService.Domain.Models.FeedbackAggregate;

public class Feedback : AggregateRoot<Id>
{
    private Feedback(
        Id id,
        Email emailFrom,
        FeedbackContent content,
        bool isWatched = false)
    {
        Id = id;
        EmailFrom = emailFrom;
        Content = content;
        IsWatched = isWatched;
    }

    public Email EmailFrom { get; private set; }
    public FeedbackContent Content { get; private set; }
    public bool IsWatched { get; set; }

    public static Result<Feedback> Create(string id, string email, string content, bool isWatched = false)
    {
        var idResult = Id.Create(id);
        if (idResult.IsFailure)
            return Result.Failure<Feedback>(idResult.Error);
        
        var emailResult = Email.Create(email);
        if (emailResult.IsFailure)
            return Result.Failure<Feedback>(emailResult.Error);
        
        var contentResult = FeedbackContent.Create(content);
        if (contentResult.IsFailure)
            return Result.Failure<Feedback>(contentResult.Error);

        return new Feedback(
            idResult.Value,
            emailResult.Value,
            contentResult.Value,
            isWatched);
    }
    
    private Feedback() {}
}