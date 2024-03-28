using ProfileService.Domain.Models.UserProfileAggregate.Snapshot;
using ProfileService.Domain.Models.UserProfileAggregate.ValueObjects;
using ProfileService.Domain.Shared;

namespace ProfileService.Domain.Models.UserProfileAggregate;

public class UserProfile : AggregateRoot<Guid>
{
    private UserProfile(
        Guid id,
        Name nickName,
        Email email,
        Age age,
        Rating rating,
        List<ProfileId> friends)
    {
        Id = id;
        NickName = nickName;
        Email = email;
        Age = age;
        Rating = rating;
        _friends = friends;
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
            Id = Id.ToString(),
            NickName = NickName.Value,
            Email = Email.Value,
            Age = Age.Value,
            FriendIds = _friends.Select(x => x.Value.ToString()).ToList(),
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
            snapshot.FriendIds);

        if (result.IsSuccess)
            return result.Value;
        return null!;
    }

    private readonly List<ProfileId> _friends = new();
    public Name NickName { get; private set; }
    public Email Email { get; private set; }
    public Age Age { get; private set; }
    public IReadOnlyList<ProfileId> Friends => _friends;
    public Rating Rating { get; private set; }

    public static Result<UserProfile> Create(string id, string nickName, string email, int age, ulong rating,
        List<string> friends)
    {
        var idResult = ProfileId.Create(id);
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

        HashSet<ProfileId> friendsIds = new();
        foreach (var friendId in friends)
        {
            var result = ProfileId.Create(friendId);
            if (result.IsSuccess)
                friendsIds.Add(result.Value);
        }

        return new UserProfile(
            idResult.Value.Value,
            nickNameResult.Value,
            emailResult.Value,
            ageResult.Value,
            ratingResult.Value,
            friendsIds.ToList());
    }
}