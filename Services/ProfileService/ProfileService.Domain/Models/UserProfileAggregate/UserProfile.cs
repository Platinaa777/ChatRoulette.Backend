using DomainDriverDesignAbstractions;
using Newtonsoft.Json;
using ProfileService.Domain.Models.Identity;
using ProfileService.Domain.Models.UserProfileAggregate.Errors;
using ProfileService.Domain.Models.UserProfileAggregate.Snapshot;
using ProfileService.Domain.Models.UserProfileAggregate.ValueObjects;

namespace ProfileService.Domain.Models.UserProfileAggregate;

public class UserProfile : AggregateRoot<Id>
{
    private UserProfile(
        Id id,
        Name nickName,
        Email email,
        Age age,
        Rating rating,
        List<Id> friends)
    {
        Id = id;
        NickName = nickName;
        Email = email;
        Age = age;
        Rating = rating;
        _friends = friends;
    }

    public void AddFriend(UserProfile friend)
    {
        _friends.Add(friend.Id);
    }

    public bool CheckIsFriend(Id profileId)
    {
        foreach (var id in _friends)
        {
            if (id == profileId)
                return true;
        }
        return false;
    }

    public Result IncreaseRating(int points)
    {
        if (points < 0)
            return Result.Failure(UserProfileErrors.CantIncreaseNegativePointsToRating);
        Result<Rating> ratingResult = Rating.Create(Rating.Value + points);
        if (ratingResult.IsFailure)
            return Result.Failure(ratingResult.Error);

        Rating = ratingResult.Value;
        return Result.Success();
    }

    public Result SetNickName(string name)
    {
        var newName = Name.Create(name);
        if (newName.IsFailure)
            return Result.Failure(newName.Error);

        NickName = newName.Value;
        return Result.Success();
    }

    public void SetEmail(string email)
    {
        var newEmail = Email.Create(email);
        if (newEmail.IsFailure)
            return;

        Email = newEmail.Value;
    }

    public UserProfileSnapshot Save()
    {
        return new UserProfileSnapshot()
        {
            Id = Id.Value.ToString(),
            NickName = NickName.Value,
            Email = Email.Value,
            Age = Age.Value,
            FriendIds = JsonConvert.SerializeObject(_friends.Select(x => x.Value.ToString()), Formatting.Indented),
            Rating = Rating.Value
        };
    }

    public static UserProfile RestoreFromSnapshot(UserProfileSnapshot snapshot)
    {
        var result = Create(
            snapshot.Id,
            snapshot.NickName,
            snapshot.Email,
            snapshot.Age,
            snapshot.Rating,
            JsonConvert.DeserializeObject<List<string>>(snapshot.FriendIds) ?? new List<string>());

        if (result.IsSuccess)
            return result.Value;
        return null!;
    }

    private readonly List<Id> _friends = new();
    public Name NickName { get; private set; }
    public Email Email { get; private set; }
    public Age Age { get; private set; }
    public IReadOnlyList<Id> Friends => _friends;
    public Rating Rating { get; private set; }

    public static Result<UserProfile> Create(string id, string nickName, string email, int age, int rating,
        List<string> friends)
    {
        var idResult = Id.Create(id);
        if (idResult.IsFailure)
            return Result.Failure<UserProfile>(idResult.Error);

        var nickNameResult = Name.Create(nickName);
        if (nickNameResult.IsFailure)
            return Result.Failure<UserProfile>(nickNameResult.Error);
        
        var emailResult = Email.Create(email);
        if (emailResult.IsFailure)
            return Result.Failure<UserProfile>(emailResult.Error);
        
        var ageResult = Age.Create(age);
        if (ageResult.IsFailure)
            return Result.Failure<UserProfile>(ageResult.Error);
        
        var ratingResult = Rating.Create(rating);
        if (ratingResult.IsFailure)
            return Result.Failure<UserProfile>(ratingResult.Error);

        HashSet<Id> friendsIds = new();
        foreach (var friendId in friends)
        {
            var result = Id.Create(friendId);
            if (result.IsSuccess)
                friendsIds.Add(result.Value);
        }

        return new UserProfile(
            idResult.Value,
            nickNameResult.Value,
            emailResult.Value,
            ageResult.Value,
            ratingResult.Value,
            friendsIds.ToList());
    }
}