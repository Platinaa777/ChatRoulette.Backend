
using DomainDriverDesignAbstractions;

namespace Chat.Domain.Entities;

public class ChatUser : AggregateRoot<string>
{
    public ChatUser(
        string id,
        string email,
        string connectionId,
        HashSet<string> previousParticipantEmails)
    {
        Id = id;
        Email = email;
        ConnectionId = connectionId;
        PreviousParticipantEmails = previousParticipantEmails;
    }
    public string Email { get; set; }
    public string ConnectionId { get; set; }
    public HashSet<string> PreviousParticipantEmails { get; private set; } = new();

    public void AddPeerToHistory(ChatUser chatUser)
    {
        if (PreviousParticipantEmails.Count >= 10)
        {
            var lastPeer = PreviousParticipantEmails.Last();
            PreviousParticipantEmails.Remove(lastPeer);
        }

        PreviousParticipantEmails.Add(chatUser.Id);
    }

    public bool CheckInHistory(string peerId)
    {
        return PreviousParticipantEmails.Contains(peerId);
    }
    private ChatUser() { }
}