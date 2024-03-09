using AuthService.DataContext.Database;
using AuthService.Domain.Models.UserAggregate.Entities;
using AuthService.Domain.Models.UserAggregate.Enumerations;
using AuthService.Domain.Models.UserAggregate.Repos;
using AuthService.Domain.Models.UserAggregate.ValueObjects;
using AuthService.Domain.Models.UserAggregate.ValueObjects.User;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Repos;

public class UserRepository : IUserRepository
{
    private readonly UserDb _context;

    public UserRepository(UserDb context)
    {
        _context = context;
    }
    
    public async Task<User?> FindUserByIdAsync(string id)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        return existingUser;
    }

    public async Task<User?> FindUserByEmailAsync(string email)
    {
        User? existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == new Email(email));
        
        return existingUser;
    }

    public async Task<bool> AddUserAsync(User user)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
        if (existingUser != null)
            return false;

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        var result = await _context.Users.Where(u => u.Email == user.Email)
            .ExecuteUpdateAsync(u1 => u1
                .SetProperty(u2 => u2.IsSubmittedEmail, u1 => true)
                .SetProperty(u2 => u2.Role, u1 => RoleType.ActivatedUser));

        return result == 1;
    }
}