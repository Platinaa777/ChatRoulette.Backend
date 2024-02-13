using Microsoft.AspNetCore.Identity;

namespace AuthService.Domain.Models;

public class UserAccount : IdentityUser
{
    public int Age { get; set; }
}