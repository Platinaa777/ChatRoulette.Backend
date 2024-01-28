namespace WaitingRoom.Core.Models;

public class TwoSeatsRoom
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public UserInfo? Host { get; set; } = null;
    public UserInfo? Participant { get; set; } = null;
}