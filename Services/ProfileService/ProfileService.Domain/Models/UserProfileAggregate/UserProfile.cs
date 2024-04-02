using DomainDriverDesignAbstractions;
using Newtonsoft.Json;
using ProfileService.Domain.Models.Identity;
using ProfileService.Domain.Models.UserProfileAggregate.Entities;
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
        HashSet<Id> friends,
        HashSet<Achievement> achievements,
        Avatar avatar)
    {
        Id = id;
        NickName = nickName;
        Email = email;
        Age = age;
        Rating = rating;
        Avatar = avatar;
        _friends = friends;
        _achievements = achievements;
    }

    public void AddFriend(UserProfile friend)
    {
        if (friend.Id != Id)
            _friends.Add(friend.Id);
    }

    public void DeleteFriend(UserProfile friend)
    {
        _friends.Remove(friend.Id);
    }

    public bool CheckIsFriend(Id profileId)
    {
        return _friends.Contains(profileId);
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

    public void ChangeAvatar(Avatar newAvatar)
    {
        Avatar = newAvatar;
    }

    public UserProfileSnapshot Save()
    {
        return new UserProfileSnapshot()
        {
            Id = Id.Value.ToString(),
            NickName = NickName.Value,
            Email = Email.Value,
            Age = Age.Value,
            Rating = Rating.Value,
            Avatar = Avatar.Value,
            FriendIds = _friends.Select(x => x.Value.ToString()).ToList(),
            AchievementSnapshots = JsonConvert.SerializeObject(_achievements.Select(x => new AchievementSnapshot()
                {
                    Id = x.Id,
                    Content = x.Content,
                    Photo = x.Photo
                }).ToList(), Formatting.Indented)
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
            snapshot.FriendIds ?? new List<string>(),
            snapshot.AchievementSnapshots ?? string.Empty,
            snapshot.Avatar ?? string.Empty);

        if (result.IsSuccess)
            return result.Value;
        return null!;
    }

    private readonly HashSet<Achievement> _achievements = new();
    private readonly HashSet<Id> _friends = new();
    public Name NickName { get; private set; }
    public Email Email { get; private set; }
    public Age Age { get; private set; }
    public IReadOnlyList<Id> Friends => _friends.ToList();
    public IReadOnlyList<Achievement> Achievements => _achievements.ToList();
    public Rating Rating { get; private set; }
    public Avatar Avatar { get; set; }

    public static Result<UserProfile> Create(
        string id,
        string nickName,
        string email,
        int age,
        int rating,
        List<string> friends,
        string jsonAchievements,
        string avatar)
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

        List<AchievementSnapshot>? achievements =
            JsonConvert.DeserializeObject<List<AchievementSnapshot>>(jsonAchievements);
        
        HashSet<Achievement> domainAchievements = new();

        if (achievements is not null)
        {
            foreach (var achievement in achievements)
            {
                var domainAchievement = Achievement.Create(
                    achievement.Id,
                    achievement.Content,
                    achievement.Photo);

                if (domainAchievement.IsSuccess)
                    domainAchievements.Add(domainAchievement.Value);
            }    
        }
        

        return new UserProfile(
            idResult.Value,
            nickNameResult.Value,
            emailResult.Value,
            ageResult.Value,
            ratingResult.Value,
            friendsIds,
            domainAchievements,
            Avatar.Create(avatar));
    }
}