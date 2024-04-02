namespace ProfileService.Infrastructure.Repos.Implementations.Profile;

public static class FriendQuery
{
    public static string UpdateFriendsTable = @"
        INSERT INTO friends_link (profile_id, friend_id)
        VALUES 
            (@Id, @FriendId);
    ";

    public static string DeleteFriendsFromTable = @"
        DELETE FROM friends_link 
        WHERE profile_id = @Id and friend_id = @FriendId;
    ";

    public static string GetAllFriends = @"
        SELECT friend_id
        FROM friends_link
        WHERE profile_id = @Id
    ";
}