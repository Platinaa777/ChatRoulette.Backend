namespace AuthService.Domain.Models.UserAggregate.Repos;

public interface IPasswordRepository
{
    Task<string> FindSaltByUserId(string id);
    Task<bool> ChangePasswordCredentials(string password, string salt, string userId);
}