using AuthService.Domain.SeedWork;

namespace AuthService.Domain.Models.UserAggregate.Enumerations;

public class RoleType : Enumeration
{
    public static RoleType Admin = new RoleType(1, nameof(Admin));
    public static RoleType UnactivatedUser = new RoleType(2, nameof(UnactivatedUser));
    public static RoleType ActivatedUser = new RoleType(3, nameof(ActivatedUser));

    private RoleType() { }
    
    protected RoleType(int id, string name) : base(id, name)
    {
    }

    public static RoleType? FromName(string name)
    {
        var collection = GetAll<RoleType>();
        foreach (var role in collection)
        {
            if (role.Name == name)
                return role;
        }

        return null;
    }
    
    public static RoleType? FromValue(int id)
    {
        var collection = GetAll<RoleType>();
        foreach (var role in collection)
        {
            if (role.Id == id)
                return role;
        }

        return null;
    }
}