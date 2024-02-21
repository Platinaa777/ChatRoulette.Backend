
using BCrypt.Net;
using BC = BCrypt.Net.BCrypt;

namespace AuthService.Infrastructure.Security;

public class PasswordHasher
{
    public static string GenerateSalt()
    {
        var salt = BC.GenerateSalt();

        return salt;
    }
    
    public static string HashPasswordWithSalt(string password, string salt)
    {
        return BC.HashPassword(password, salt, false, HashType.SHA256);
    }

    public static bool Verify(string passwordDb, string requestPassword)
    {
        return passwordDb == requestPassword;
    }
}