using DomainDriverDesignAbstractions;

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
    public static readonly Error RatingShouldBePositive = new("UserProfile.Error", "Rating of the user cant be negative");
    public static readonly Error CantIncreaseNegativePointsToRating = new("UserProfile.Error.Rating", "Cant subtract some points from rating");
    public static readonly Error FriendDoesNotExist = new("UserProfile.Error.Rating", "This friends does not exist in your friends list");
    public static readonly Error CantRemoveUserFromFriends = new("UserProfile.Error.Rating", "Cant remove user from friends list");
    public static readonly Error AvatarUploadError = new("UserProfile.Error.Avatar", "Cant upload avatar in the system");
    public static readonly Error AvatarDoesNotExists = new("UserProfile.Error.Avatar", "Avatar cant be update because does not exist");


    public UserProfileErrors(string code, string message) : base(code, message)
    {
    }
}