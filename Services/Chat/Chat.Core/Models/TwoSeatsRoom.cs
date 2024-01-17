namespace Chat.Core.Models;

public class TwoSeatsRoom
{
    public string Id { get; set; }
    public int Duration { get; set; }
    public byte CountUsersInRoom { get; set; }
    public string FirstTalker { get; set; } = null;
    public string SecondTalker { get; set; } = null;
}