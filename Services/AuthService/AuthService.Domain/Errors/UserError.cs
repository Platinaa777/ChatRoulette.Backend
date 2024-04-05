using DomainDriverDesignAbstractions;

namespace AuthService.Domain.Errors.UserErrors;

public class UserError : Error
{
    public static readonly Error UnactivatedUser = new("User.Error.User", "Unactivated user");
    public static readonly Error WrongPassword = new("User.Error.Security", "Wrong password");
    public static readonly Error InvalidAge = new("User.Error", "Invalid age");
    public static readonly Error InvalidEmail = new("User.Error", "Invalid email");
    public static readonly Error InvalidAgeIncrease = new("User.Error.Age", "Invalid age increase. Cant be less or equal than zero");
    public static readonly Error EmptyName = new("User.Error.Name", "Empty name");
    public static readonly Error SmallPassword = new("User.Error.Security", "Small password, should be greater than 5");
    public static readonly Error UserAlreadyExist = new("User.Error", "User already exist");
    public static readonly Error UserNotFound = new("User.Error", "User not found");
    public static readonly Error CantUpdateUser = new("User.Error", "User cat be updated");
    public static readonly Error InvalidId = new("User.Error", "Invalid id");
    public static readonly Error BanUser = new("User.Error", "User was banned");


    public UserError(string code, string message) : base(code, message)
    {
    }
}