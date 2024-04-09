using DomainDriverDesignAbstractions;
using ProfileService.Domain.Models.Identity;
using ProfileService.Domain.Models.UserHistoryAggregate.Errors;
using ProfileService.Domain.Models.UserHistoryAggregate.Events;
using ProfileService.Domain.Models.UserHistoryAggregate.Snapshots;

namespace ProfileService.Domain.Models.UserHistoryAggregate;

public class UserHistory : AggregateRoot<Id>
{
    private UserHistory(
        Id id,
        Id userId,
        int doomSlayerPoints,
        int avatarPoints)
    {
        Id = id;
        UserId = userId;
        DoomSlayerPoints = doomSlayerPoints;
        AvatarPoints = avatarPoints;
    }
    public int DoomSlayerPoints { get; private set; }
    public int AvatarPoints { get; private set; }
    public Id UserId { get; init; }

    public void IncreaseDoomSlayerPoints()
    {
        DoomSlayerPoints++;
    }
    
    public void IncreaseAvatarPoints()
    {
        AvatarPoints++;
        if (AvatarPoints >= 25)
        {
            RaiseDomainEvent(new ReceivedAvatarAchievementDomainEvent(UserId.Value.ToString()));
        }
    }
    
    public UserHistorySnapshot Save()
    {
        return new UserHistorySnapshot()
        {
            Id = Id.Value.ToString(),
            UserId = UserId.Value.ToString(),
            DoomSlayerPoints = DoomSlayerPoints,
            AvatarPoints = AvatarPoints
        };
    }

    public static UserHistory RestoreFromSnapshot(UserHistorySnapshot snapshot)
    {
        var result = Create(
            snapshot.Id,
            snapshot.UserId,
            snapshot.DoomSlayerPoints,
            snapshot.AvatarPoints);

        if (result.IsSuccess)
            return result.Value;
        return null!;
    }

    public static Result<UserHistory> Create(
        string id,
        string userId,
        int doomSlayerPoints,
        int avatarPoints)
    {
        var idResult = Id.Create(id);
        if (idResult.IsFailure)
            return Result.Failure<UserHistory>(idResult.Error);

        var userIdResult = Id.Create(userId);
        if (userIdResult.IsFailure)
            return Result.Failure<UserHistory>(userIdResult.Error);
        
        if (doomSlayerPoints < 0)
            return Result.Failure<UserHistory>(UserHistoryError.NegativeDoomSlayerPoints);
        
        if (avatarPoints < 0)
            return Result.Failure<UserHistory>(UserHistoryError.NegativeAvatarPoints);

        return new UserHistory(
            idResult.Value,
            userIdResult.Value,
            doomSlayerPoints,
            avatarPoints);
    }
}