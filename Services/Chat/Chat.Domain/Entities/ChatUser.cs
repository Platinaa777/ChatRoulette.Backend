
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
    public string Email { get; private set; }
    public string ConnectionId { get; private set; }
    public HashSet<string> PreviousParticipantEmails { get; private set; } = new();

    public void RefreshConnectionId(string newConnectionId)
    {
        ConnectionId = newConnectionId;
    }

    public void AddPeerToHistory(ChatUser chatUser)
    {
        if (PreviousParticipantEmails.Count >= 3)
        {
            var lastPeer = PreviousParticipantEmails.First();
            PreviousParticipantEmails.Remove(lastPeer);
        }

        PreviousParticipantEmails.Add(chatUser.Email);
    }

    public bool CheckInHistory(string peerEmail)
    {
        return PreviousParticipantEmails.Contains(peerEmail);
    }
    private ChatUser() { }
    public const int MaxLenUserHistory = 3;
}