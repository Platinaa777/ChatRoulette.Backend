using Chat.Domain.SeedWork;

namespace Chat.Domain.Entities;

public class ChatUser : ValueObject
{
    public ChatUser(string email, string connectionId)
    {
        Email = email;
        ConnectionId = connectionId;
    }
    
    public string Email { get; set; }
    public string ConnectionId { get; set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Email;
        yield return ConnectionId;
    }
}