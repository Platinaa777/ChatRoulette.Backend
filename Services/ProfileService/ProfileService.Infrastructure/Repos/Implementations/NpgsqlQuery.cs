namespace ProfileService.Infrastructure.Repos.Implementations;

public static class NpgsqlQuery
{
    public const string sqlFindByEmail = $@"
            SELECT id, nick_name as NickName, email Email, age Age, preferences Preferences
            FROM user_profiles
            WHERE email = @email;
         ";

    public const string sqlFindById = $@"
            SELECT id, nick_name as NickName, email Email, age Age, preferences Preferences
            FROM user_profiles
            WHERE id = @id
        ";

    public const string sqlAddUser = $@"
        INSERT INTO user_profiles (id, nick_name, email, age, preferences)
        VALUES 
            (@id, @NickName, @Email, @Age, @Preferences);
        ";
    
    public const string sqlUpdateUser = $@"
        UPDATE user_profiles
        SET
            nick_name = @NickName,
            email = @Email,
            age = @Age,
            preferences = @Preferences
        WHERE email = @Email
        ";
}