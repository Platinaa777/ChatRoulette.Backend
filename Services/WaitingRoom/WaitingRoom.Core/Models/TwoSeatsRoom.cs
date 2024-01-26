namespace WaitingRoom.Core.Models;

public class TwoSeatsRoom
{
    public string Id { get; set; }
    public List<UserInfo> Talkers { get; } = new();
    public bool IsInitial { get; set; } = true;
}