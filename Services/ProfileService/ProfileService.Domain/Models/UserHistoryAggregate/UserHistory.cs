using DomainDriverDesignAbstractions;
using ProfileService.Domain.Models.Identity;
using ProfileService.Domain.Models.UserHistoryAggregate.Errors;
using ProfileService.Domain.Models.UserHistoryAggregate.Snapshots;

namespace ProfileService.Domain.Models.UserHistoryAggregate;

public class UserHistory : AggregateRoot<Id>
{
    private UserHistory(
        Id id,
        Id userId,
        int doomSlayerPoints)
    {
        Id = id;
        DoomSlayerPoints = doomSlayerPoints;
    }
    public int DoomSlayerPoints { get; private set; }
    public Id UserId { get; init; }

    public void IncreaseDoomSlayerPoints()
    {
        DoomSlayerPoints++;
    }
    
    public UserHistorySnapshot Save()
    {
        return new UserHistorySnapshot()
        {
            Id = Id.Value.ToString(),
            UserId = UserId.Value.ToString(),
            DoomSlayerPoints = DoomSlayerPoints
        };
    }

    public static UserHistory RestoreFromSnapshot(UserHistorySnapshot snapshot)
    {
        var result = Create(
            snapshot.Id,
            snapshot.UserId,
            snapshot.DoomSlayerPoints);

        if (result.IsSuccess)
            return result.Value;
        return null!;
    }

    public static Result<UserHistory> Create(string id, string userId, int doomSlayerPoints)
    {
        var idResult = Id.Create(id);
        if (idResult.IsFailure)
            return Result.Failure<UserHistory>(idResult.Error);

        var userIdResult = Id.Create(userId);
        if (userIdResult.IsFailure)
            return Result.Failure<UserHistory>(userIdResult.Error);
        
        if (doomSlayerPoints < 0)
            return Result.Failure<UserHistory>(UserHistoryError.NegativeDoomSlayerPoints);

        return new UserHistory(idResult.Value, userIdResult.Value, doomSlayerPoints);
    }
}