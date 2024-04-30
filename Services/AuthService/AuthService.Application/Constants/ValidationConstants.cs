namespace AuthService.Application.Constants;

public class ValidationConstants
{
    public const string InvalidEmail = "Email is not valid";
    public const string BanShouldBeGreaterThanZero = "Time to ban (in minutes) should be greater that 0 minutes";
    public const string NotEnoughLettersInUserName = "Username should has more than 6 letters"; 
    public const string NotEnoughLettersInPassword = "Password should has more than 4 letters";
    public const string EmptyRefreshToken = "The size of refresh token should be not empty"; 
}