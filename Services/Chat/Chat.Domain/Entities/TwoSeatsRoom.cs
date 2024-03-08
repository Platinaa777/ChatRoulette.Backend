using Chat.Domain.SeedWork;

namespace Chat.Domain.Entities;

public class TwoSeatsRoom : Entity
{
    public TwoSeatsRoom()
    {
        Id = Guid.NewGuid().ToString();
        Peers = new List<ChatUser?>();
    }

    public readonly List<ChatUser?> Peers;
}