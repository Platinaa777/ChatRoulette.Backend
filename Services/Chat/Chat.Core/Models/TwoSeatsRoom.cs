namespace Chat.Core.Models;

public class TwoSeatsRoom
{
    public string Id { get; set; }
    public int Duration { get; set; }
    public List<string> Talkers { get; } = new();
    public bool IsInitial { get; set; } = true;
    public string ConnectionString { get; set; }
}