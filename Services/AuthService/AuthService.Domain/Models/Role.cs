using AuthService.Domain.SeedWork;

namespace AuthService.Domain.Models;

public class Role : Enumeration
{
    public static Role Admin = new Role(1, nameof(Admin));
    public static Role UnactivatedUser = new Role(2, nameof(UnactivatedUser));
    public static Role ActivatedUser = new Role(3, nameof(ActivatedUser));

    protected Role(int id, string name) : base(id, name)
    {
    }

    public static Role? FromName(string name)
    {
        var collection = GetAll<Role>();
        foreach (var role in collection)
        {
            if (role.Name == name)
                return role;
        }

        return null;
    }
    
    public static Role? FromValue(int id)
    {
        var collection = GetAll<Role>();
        foreach (var role in collection)
        {
            if (role.Id == id)
                return role;
        }

        return null;
    }
}