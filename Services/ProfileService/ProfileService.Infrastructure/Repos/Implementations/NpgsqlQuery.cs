namespace ProfileService.Infrastructure.Repos.Implementations;

public static class NpgsqlQuery
{
    public const string sqlFindByEmail = $@"
            SELECT id, user_name UserName, nick_name NickName, email 
            FROM user_profiles
            WHERE email = @email;
         ";

    public const string sqlFindById = $@"
            SELECT id, user_name as UserName, nick_name as NickName, email
            FROM user_profiles
            WHERE id = @id
        ";

    public const string sqlAddUser = $@"
        INSERT INTO user_profiles (id, user_name, nick_name, email)
        VALUES 
            (@id, @UserName, @NickName, @email);
        ";
    
    public const string sqlUpdateUser = $@"
        UPDATE user_profiles
        SET
            user_name = @UserName,
            nick_name = @NickName,
            email = @Email
        WHERE email = @Email
        ";
}