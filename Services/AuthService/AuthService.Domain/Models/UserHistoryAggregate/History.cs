using AuthService.Domain.Errors;
using AuthService.Domain.Errors.UserErrors;
using AuthService.Domain.Models.Shared;
using DomainDriverDesignAbstractions;

namespace AuthService.Domain.Models.UserHistoryAggregate;

public class History : AggregateRoot<Id>
{
    public static Result<History> Create(
        string historyId,
        string userId,
        DateTime? bannedTime = default)
    {
        var historyIdResult = Id.CreateId(historyId);
        if (historyIdResult.IsFailure)
            return Result.Failure<History>(HistoryError.InvalidHistoryId);

        var userIdResult = Id.CreateId(userId);
        if (userIdResult.IsFailure)
            return Result.Failure<History>(UserError.InvalidId);

        if (bannedTime is not null && bannedTime < DateTime.UtcNow)
        {
            return new History(
                historyIdResult.Value,
                userIdResult.Value,
                null);
        }
        
        return new History(
            historyIdResult.Value,
            userIdResult.Value,
            bannedTime);
    }
    
    private History(
        Id id,
        Id userId,
        DateTime? bannedTime)
    {
        UserId = userId;
        BannedTime = bannedTime;
        Id = id;
    }

    public Id UserId { get; private set; }
    public DateTime? BannedTime { get; private set; }

    public void BanUser(int minutes)
    {
        BannedTime = DateTime.UtcNow.AddMinutes(minutes);
    }

    public void DisbandUser()
    {
        BannedTime = null;
    }
}