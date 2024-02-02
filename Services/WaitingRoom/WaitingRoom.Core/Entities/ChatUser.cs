namespace WaitingRoom.Core.Models;

public class ChatUser : IEquatable<ChatUser>
{
    public string Email { get; set; }
    public string ConnectionId { get; set; }
    public bool Equals(ChatUser? other)
    {
        return Email == other.Email;
    }
}