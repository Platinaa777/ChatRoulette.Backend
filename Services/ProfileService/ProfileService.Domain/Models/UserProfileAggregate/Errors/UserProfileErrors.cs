using ProfileService.Domain.Shared;

namespace ProfileService.Domain.Models.UserProfileAggregate.Errors;

public class UserProfileErrors : Error
{
    public static readonly Error InvalidAge = new("UserProfile.Error", "Invalid age");
    public static readonly Error InvalidEmail = new("UserProfile.Error", "Invalid email");
    public static readonly Error InvalidAgeIncrease = new("UserProfile.Error.Age", "Invalid age increase. Cant be less or equal than zero");
    public static readonly Error EmptyName = new("UserProfile.Error.Name", "Empty name");
    public static readonly Error UserNotFound = new("UserProfile.Error", "User not found");
    public static readonly Error CantUpdateUser = new("UserProfile.Error", "User cat be updated");
    public static readonly Error InvalidProfileId = new("UserProfile.Error", "Cant parse id of the user");
    public static readonly Error EmailAlreadyExist = new("UserProfile.Error", "User with this email is already exist");
    public static readonly Error CantAddUserProfile = new("UserProfile.Error", "User cant be added to the system");
    public static readonly Error EmailNotFound = new("UserProfile.Error", "User with this email not found");
    
    public UserProfileErrors(string code, string message) : base(code, message)
    {
    }
}