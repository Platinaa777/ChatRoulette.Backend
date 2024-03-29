using Chat.Domain.SeedWork;

namespace Chat.Domain.Entities;

public class TwoSeatsRoom : Entity
{
    public TwoSeatsRoom()
    {
        Id = Guid.NewGuid().ToString();
        Peers = new List<ChatUser?>();
        _createdAt = DateTime.Now;
    }

    public int Close()
    {
        _closedAt = DateTime.Now;
        return _closedAt.Subtract(_createdAt).Minutes;
    }

    public readonly List<ChatUser?> Peers;
    private readonly DateTime _createdAt;
    private DateTime _closedAt;
}