using DomainDriverDesignAbstractions;
using Newtonsoft.Json;
using ProfileService.Domain.Models.Identity;
using ProfileService.Domain.Models.UserProfileAggregate.Entities;
using ProfileService.Domain.Models.UserProfileAggregate.Errors;
using ProfileService.Domain.Models.UserProfileAggregate.Events;
using ProfileService.Domain.Models.UserProfileAggregate.Snapshot;
using ProfileService.Domain.Models.UserProfileAggregate.ValueObjects;

namespace ProfileService.Domain.Models.UserProfileAggregate;

public class UserProfile : AggregateRoot<Id>
{
    private UserProfile(
        Id id,
        Name userName,
        Email email,
        BirthDateUtc birthDateUtc,
        Rating rating,
        HashSet<Id> friends,
        HashSet<Achievement> achievements,
        Avatar avatar)
    {
        Id = id;
        UserName = userName;
        Email = email;
        BirthDateUtc = birthDateUtc;
        Rating = rating;
        Avatar = avatar;
        _friends = friends;
        _achievements = achievements;
    }

    public void AddFriend(UserProfile friend)
    {
        if (friend.Id != Id)
        {
            _friends.Add(friend.Id);
            if (_friends.Count >= 5)
            {
                RaiseDomainEvent(new GotManyFriendsDomainEvent(Id.Value.ToString()));
            }
        }
    }

    public void AddAchievement(Achievement achievement)
    {
        _achievements.Add(achievement);
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

        if (Rating.Value >= 250 && _achievements.All(x => x.Id != 4))
        {
            RaiseDomainEvent(new Got250RatingOnProfileDomainEvent(Id.Value.ToString()));
        }
        
        return Result.Success();
    }

    public Result SetNickName(string name)
    {
        var newName = Name.Create(name);
        if (newName.IsFailure)
            return Result.Failure(newName.Error);

        UserName = newName.Value;
        return Result.Success();
    }

    public void ChangeAvatar(string newAvatar)
    {
        Avatar = Avatar.Create(newAvatar);
        RaiseDomainEvent(new ChangedAvatarDomainEvent(Id.Value.ToString()));
    }

    public void RefreshAvatar(string avatar)
    {
        Avatar = Avatar.Create(avatar);
    }

    public UserProfileSnapshot Save()
    {
        return new UserProfileSnapshot()
        {
            Id = Id.Value.ToString(),
            UserName = UserName.Value,
            Email = Email.Value,
            BirthDateUtc = BirthDateUtc.Value,
            Rating = Rating.Value,
            Avatar = Avatar.Value,
            FriendIds = _friends.Select(x => x.Value.ToString()).ToList(),
            AchievementSnapshots = JsonConvert.SerializeObject(_achievements.Select(x => new AchievementSnapshot()
                {
                    Id = x.Id,
                    Photo = x.Photo
                }).ToList(), Formatting.Indented)
        };
    }

    public static UserProfile RestoreFromSnapshot(UserProfileSnapshot snapshot)
    {
        var result = Create(
            snapshot.Id,
            snapshot.UserName,
            snapshot.Email,
            snapshot.BirthDateUtc,
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
    public Name UserName { get; private set; }
    public Email Email { get; private set; }
    public BirthDateUtc BirthDateUtc { get; private set; }
    public IReadOnlyList<Id> Friends => _friends.ToList();
    public IReadOnlyList<Achievement> Achievements => _achievements.ToList();
    public Rating Rating { get; private set; }
    public Avatar Avatar { get; private set; }

    public static Result<UserProfile> Create(
        string id,
        string nickName,
        string email,
        DateTime birthDateUtc,
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
        
        var birthDateUtcResult = BirthDateUtc.Create(birthDateUtc);
        if (birthDateUtcResult.IsFailure)
            return Result.Failure<UserProfile>(birthDateUtcResult.Error);
        
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
                    achievement.Photo);

                if (domainAchievement.IsSuccess)
                    domainAchievements.Add(domainAchievement.Value);
            }    
        }
        

        return new UserProfile(
            idResult.Value,
            nickNameResult.Value,
            emailResult.Value,
            birthDateUtcResult.Value,
            ratingResult.Value,
            friendsIds,
            domainAchievements,
            Avatar.Create(avatar));
    }
}