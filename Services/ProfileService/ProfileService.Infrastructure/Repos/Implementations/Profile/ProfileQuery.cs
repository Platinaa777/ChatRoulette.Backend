namespace ProfileService.Infrastructure.Repos.Implementations.Profile;

public static class ProfileQuery
{
    public const string SqlFindByEmail = $@"
            SELECT id, nick_name as NickName, email Email, age Age, rating Rating, friends FriendIds
            FROM user_profiles
            WHERE email = @email;
         ";

    public const string SqlFindById = $@"
            SELECT id, nick_name as NickName, email Email, age Age, rating Rating, friends FriendIds
            FROM user_profiles
            WHERE id = @id
        ";

    public const string SqlAddUser = $@"
        INSERT INTO user_profiles (id, nick_name, email, age, rating, friends)
        VALUES 
            (@id, @NickName, @Email, @Age, @Rating, @FriendIds);
        ";
    
    public const string SqlUpdateUser = $@"
        UPDATE user_profiles
        SET
            nick_name = @NickName,
            email = @Email,
            age = @Age,
            rating = @Rating,
            friends = @FriendIds
        WHERE email = @Email
        ";
    
    public const string SqlGetAllUsers = $@"
            SELECT id, nick_name as NickName, email Email, age Age, rating Rating, friends FriendIds
            FROM user_profiles;
         ";
}