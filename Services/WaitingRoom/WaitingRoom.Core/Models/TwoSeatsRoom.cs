namespace WaitingRoom.Core.Models;

public class TwoSeatsRoom
{
    public string Id { get; set; }
    public UserInfo Host { get; set; }
    public UserInfo Participant { get; set; }
    public bool IsInitial { get; set; } = true;
}