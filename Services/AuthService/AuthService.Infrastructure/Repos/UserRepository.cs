using AuthService.DataContext.Database;
using AuthService.Domain.Models.Shared;
using AuthService.Domain.Models.UserAggregate;
using AuthService.Domain.Models.UserAggregate.Enumerations;
using AuthService.Domain.Models.UserAggregate.Repos;
using AuthService.Domain.Models.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Repos;

public class UserRepository : IUserRepository
{
    private readonly UserDb _context;

    public UserRepository(UserDb context)
    {
        _context = context;
    }
    
    public async Task<User?> FindUserByIdAsync(Id id)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        return existingUser;
    }

    public async Task<User?> FindUserByEmailAsync(Email email)
    {
        User? existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
        
        return existingUser;
    }

    public async Task<bool> AddUserAsync(User user)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email || u.UserName == user.UserName);
        if (existingUser != null)
            return false;

        await _context.Users.AddAsync(user);
        return true;
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        var result = await _context.Users.Where(u => u.Email == user.Email)
            .ExecuteUpdateAsync(entity => entity
                .SetProperty(email => email.IsSubmittedEmail, user.IsSubmittedEmail)
                .SetProperty(r => r.Role, RoleType.ActivatedUser));

        return result == 1;
    }
}