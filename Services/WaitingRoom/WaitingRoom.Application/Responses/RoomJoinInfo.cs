using WaitingRoom.Core.Models;

namespace WaitingRoom.Application.Responses;

public class RoomJoinInfo
{
    public string roomId { get; set; }
    public ChatUser ChatUser { get; set; }
    public bool IsHost { get; set; }
}