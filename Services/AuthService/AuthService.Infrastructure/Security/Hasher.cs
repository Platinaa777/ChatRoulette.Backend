
using AuthService.Application.Security;
using BCrypt.Net;
using BC = BCrypt.Net.BCrypt;

namespace AuthService.Infrastructure.Security;

public class Hasher : IHasherPassword
{
    public string GenerateSalt()
    {
        var salt = BC.GenerateSalt();

        return salt;
    }
    
    public string HashPasswordWithSalt(string password, string salt)
    {
        return BC.HashPassword(password, salt, false, HashType.SHA256);
    }

    public bool Verify(string passwordDb, string requestPassword)
    {
        return passwordDb == requestPassword;
    }
}