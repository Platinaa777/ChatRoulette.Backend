namespace AuthService.Application.Security;

public interface IHasherPassword
{
    public string GenerateSalt();
    public string HashPasswordWithSalt(string password, string salt);
    public bool Verify(string passwordDb, string requestPassword);
}