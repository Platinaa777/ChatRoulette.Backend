using AuthService.Domain.Models.UserAggregate.Enumerations;
using AuthService.Domain.SeedWork;

namespace AuthService.Domain.Models.UserAggregate.Entities;

public class Role : Entity<int>
{
    public RoleType Value { get; set; }
    
    public Role() { }
    public Role(int id)
    {
        Value = RoleType.FromValue(id)!;
    }
}