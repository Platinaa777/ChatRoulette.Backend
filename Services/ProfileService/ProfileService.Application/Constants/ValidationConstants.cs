namespace ProfileService.Application.Constants;

public static class ValidationConstants
{
    public const string InvalidEmail = "Email is not valid";
    public const string NotEnoughLettersInUserName = "Username should has more than 6 letters"; 
    public const string SenderEmailInvalid = "Invalid email for sender email field";
    public const string ReceiverEmailInvalid = "Invalid email for receiver of invitation";
    public const string EqualitySenderAndReceiver = "You cant send invitation to friend to yourself";
    public const string EmptyIdOfProfile = "Empty id field. Should be not empty";
    public const string RatingPointShouldBePositive = "Rating points should be positive value";
    public const string EmptyAvatar = "Photo of avatar should has a value. Must not be empty";
    public const string PeerListEmpty = "Peer list is empty";
}