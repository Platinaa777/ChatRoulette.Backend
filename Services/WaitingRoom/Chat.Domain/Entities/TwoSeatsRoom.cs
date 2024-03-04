namespace Chat.Domain.Entities;

public class TwoSeatsRoom
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public List<ChatUser?> peers = new List<ChatUser?>();
}