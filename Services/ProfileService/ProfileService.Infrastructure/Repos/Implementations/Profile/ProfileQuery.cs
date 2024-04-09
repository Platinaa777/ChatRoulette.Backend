namespace ProfileService.Infrastructure.Repos.Implementations.Profile;

public static class ProfileQuery
{
    public const string SqlFindByEmail = $@"
            SELECT id, nick_name as NickName, email Email, age Age, rating Rating, avatar Avatar, achievements AchievementSnapshots, fl.profile_id as ProfileId, fl.friend_id
            FROM 
                user_profiles u LEFT JOIN friends_link fl ON u.id = fl.profile_id
            WHERE email = @email;
         ";

    public const string SqlFindById = $@"
            SELECT id, nick_name as NickName, email Email, age Age, rating Rating, avatar Avatar, achievements AchievementSnapshots, fl.profile_id as ProfileId, fl.friend_id
            FROM 
                user_profiles u LEFT JOIN friends_link fl ON u.id = fl.profile_id
            WHERE id = @id
        ";

    public const string SqlAddUser = $@"
        INSERT INTO user_profiles (id, nick_name, email, age, rating, avatar, achievements)
        VALUES 
            (@id, @NickName, @Email, @Age, @Rating, @Avatar, @AchievementSnapshots);
        ";
    
    public const string SqlUpdateUser = $@"
        UPDATE user_profiles
        SET
            nick_name = @NickName,
            email = @Email,
            age = @Age,
            rating = @Rating,
            avatar = @Avatar,
            achievements = @AchievementSnapshots
        WHERE email = @Email
        ";
    
    public const string SqlGetAllUsers = $@"
            SELECT id Id, nick_name as NickName, email Email, age Age, rating Rating, avatar Avatar, achievements AchievementSnapshots, fl.profile_id as ProfileId, fl.friend_id
            FROM 
                user_profiles u LEFT JOIN friends_link fl ON u.id = fl.profile_id
         ";
}