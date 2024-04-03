using DomainDriverDesignAbstractions;

namespace Chat.Domain.Entities;

public class TwoSeatsRoom : AggregateRoot<string>
{
    public TwoSeatsRoom(string id, List<string> peerEmails, DateTime createdAt)
    {
        Id = id;
        PeerEmails = peerEmails;
        CreatedAt = createdAt;
    }

    public int Close()
    {
        DateTime temp = DateTime.Now;
        ClosedAt = temp;
        return temp.Subtract(CreatedAt).Minutes;
    }

    public void AddPeer(string peerId)
    {
        PeerEmails.Add(peerId);
    }

    public List<string> PeerEmails { get; private set; } 
    public DateTime CreatedAt { get; private set; }
    public DateTime? ClosedAt { get; private set; }
}