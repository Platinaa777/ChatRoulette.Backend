namespace ProfileService.Infrastructure.Repos.Implementations.Profile;

public static class ProfileQuery
{
    public const string SqlFindByEmail = $@"
            SELECT id, user_name as UserName, email Email, birthdate BirthDateUtc, rating Rating, avatar Avatar, achievements AchievementSnapshots, fl.profile_id as ProfileId, fl.friend_id
            FROM 
                user_profiles u LEFT JOIN friends_link fl ON u.id = fl.profile_id
            WHERE email = @email;
         ";

    public const string SqlFindById = $@"
            SELECT id, user_name as UserName, email Email, birthdate BirthDateUtc, rating Rating, avatar Avatar, achievements AchievementSnapshots, fl.profile_id as ProfileId, fl.friend_id
            FROM 
                user_profiles u LEFT JOIN friends_link fl ON u.id = fl.profile_id
            WHERE id = @id
        ";

    public const string SqlAddUser = $@"
        INSERT INTO user_profiles (id, user_name, email, birthdate, rating, avatar, achievements)
        VALUES 
            (@id, @UserName, @Email, @BirthDateUtc, @Rating, @Avatar, @AchievementSnapshots);
        ";
    
    public const string SqlUpdateUser = $@"
        UPDATE user_profiles
        SET
            user_name = @UserName,
            email = @Email,
            birthdate = @BirthDateUtc,
            rating = @Rating,
            avatar = @Avatar,
            achievements = @AchievementSnapshots
        WHERE email = @Email
        ";
    
    public const string SqlGetAllUsers = $@"
            SELECT id Id, user_name as UserName, email Email, birthdate BirthDateUtc, rating Rating, avatar Avatar, achievements AchievementSnapshots, fl.profile_id as ProfileId, fl.friend_id
            FROM 
                user_profiles u LEFT JOIN friends_link fl ON u.id = fl.profile_id
         ";
}