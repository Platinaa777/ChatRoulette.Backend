using Chat.Domain.ValueObjects;
using DomainDriverDesignAbstractions;

namespace Chat.Domain.Aggregates.Room;

public class TwoSeatsRoom : AggregateRoot<string>
{
    public TwoSeatsRoom(string id, List<UserLink> peerLinks, DateTime createdAt)
    {
        Id = id;
        PeerLinks = peerLinks;
        CreatedAt = createdAt;
    }

    public bool CanCreateOffer()
    {
        return PeerLinks.Count == 2;
    }

    public int Close()
    {
        DateTime temp = DateTime.UtcNow;
        ClosedAt = temp;
        return temp.Subtract(CreatedAt).Minutes;
    }

    public void AddPeer(ChatUser peer)
    {
        var userLink = peer.ToUserLink();

        if (!PeerLinks.Contains(userLink) && !IsFullRoom())
        {
            PeerLinks.Add(userLink);    
        }
    }

    public bool ContainsPeerConnectionId(string connectionId)
    {
        foreach (var userLink in PeerLinks)
        {
            if (userLink.ConnectionId == connectionId)
                return true;
        }

        return false;
    }

    public bool IsFullRoom() => PeerLinks.Count == 2;
    

    public List<UserLink> PeerLinks { get; private set; } 
    public DateTime CreatedAt { get; private set; }
    public DateTime? ClosedAt { get; private set; }
}