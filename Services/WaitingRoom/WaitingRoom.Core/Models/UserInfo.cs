namespace WaitingRoom.Core.Models;

public class UserInfo : IEquatable<UserInfo>
{
    public string Email { get; set; }

    public bool Equals(UserInfo? other)
    {
        return Email == other.Email;
    }
}