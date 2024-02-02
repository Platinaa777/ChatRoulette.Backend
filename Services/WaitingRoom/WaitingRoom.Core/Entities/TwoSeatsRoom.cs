namespace WaitingRoom.Core.Models;

public class TwoSeatsRoom
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public ChatUser? Host { get; set; } = null;
    public ChatUser? Participant { get; set; } = null;
}