using AuthService.DataContext.Database;
using AuthService.Domain.Models.UserAggregate.Repos;
using AuthService.Domain.Models.UserAggregate.ValueObjects;
using AuthService.Domain.Models.UserAggregate.ValueObjects.User;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Repos;

public class PasswordRepository : IPasswordRepository
{
    private readonly UserDb _context;

    public PasswordRepository(UserDb context)
    {
        _context = context;
    }
    
    public async Task<string> FindSaltByUserId(string id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            throw new ArgumentException("User not found");
        
        return user.Salt.Value;
    }

    public async Task<bool> ChangePasswordCredentials(string password, string salt, string userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
            return false;

        var rowsAffected = await _context.Users
                .Where(userDb => userDb.Id == userId)
                .ExecuteUpdateAsync(u => u
                .SetProperty(u1 => u1.PasswordHash, u1 => new Password(password))
                .SetProperty(u2 => u2.Salt, u2 => new Salt(salt)));

        return rowsAffected == 1;
    }
}