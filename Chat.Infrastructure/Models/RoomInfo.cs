using System.Collections.Concurrent;

namespace Chat.Infrastructure.Models;

public class RoomInfo
{
    public int CountParticipant { get; set; }
    public ConcurrentBag<string> Participants { get; set; } = new();
}