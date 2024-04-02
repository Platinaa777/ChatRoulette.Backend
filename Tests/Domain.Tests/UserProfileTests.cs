using DomainDriverDesignAbstractions;
using ProfileService.Domain.Models.UserProfileAggregate;

namespace Domain.Tests;

public class UserProfileTests
{
    [Fact]
    public void HandleAddToFriendsList_WhenTwoFriendsEqual_ShouldBeOneFriend()
    {
        Result<UserProfile> profileResult = UserProfile.Create(
            Guid.NewGuid().ToString(),
            "test-nick",
            "test@mail.ru",
            19,
            0,
            new List<string>(),
            "",
            "");

        Assert.True(profileResult.IsSuccess);

        Result<UserProfile> friendResult = UserProfile.Create(
            Guid.NewGuid().ToString(),
            "friend-nick",
            "friend@mail.ru",
            18,
            5,
            new List<string>(),
            "",
            "");

        Assert.True(friendResult.IsSuccess);

        profileResult.Value.AddFriend(friendResult.Value);
        friendResult.Value.AddFriend(profileResult.Value);

        Assert.Equal(1, profileResult.Value.Friends.Count);
        Assert.Equal(1, friendResult.Value.Friends.Count);

        profileResult.Value.AddFriend(friendResult.Value);
        friendResult.Value.AddFriend(profileResult.Value);

        Assert.Equal(1, profileResult.Value.Friends.Count);
        Assert.Equal(1, friendResult.Value.Friends.Count);
    }

    [Fact]
    public void HandleAddToAchievementsList_WhenTwoAchievementsEqual_ShouldBeOneAchievement()
    {
        
    }
}