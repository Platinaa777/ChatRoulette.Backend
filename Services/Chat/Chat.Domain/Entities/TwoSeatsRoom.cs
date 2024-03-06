using Chat.Domain.SeedWork;

namespace Chat.Domain.Entities;

public class TwoSeatsRoom : Entity<string>
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public List<ChatUser?> peers = new List<ChatUser?>();
}