using WaitingRoom.Core.Models;

namespace WaitingRoom.Application.Responses;

public class RoomJoinInfo
{
    public string roomId { get; set; }
    public UserInfo User { get; set; }
    public bool IsHost { get; set; }
}