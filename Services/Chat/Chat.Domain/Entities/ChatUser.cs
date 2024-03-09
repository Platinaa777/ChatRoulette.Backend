using Chat.Domain.SeedWork;

namespace Chat.Domain.Entities;

public class ChatUser : Entity
{
    public ChatUser(string email, string connectionId)
    {
        Email = email;
        Id = connectionId;
        PreviousParticipantIds = new HashSet<string>();
    }
    public string Email { get; set; }
    public HashSet<string> PreviousParticipantIds { get; set; }
}