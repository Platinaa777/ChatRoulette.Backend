using AuthService.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthService.DataContext.Database;

public class UserDb : IdentityDbContext<UserAccount, IdentityRole, string>
{
    public UserDb(DbContextOptions<UserDb> options) : base(options)
    {
    }

    public DbSet<UserAccount> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
