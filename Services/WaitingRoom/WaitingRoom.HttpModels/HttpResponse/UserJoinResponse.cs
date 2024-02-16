using Chat.Application.Models;

namespace WaitingRoom.HttpModels.HttpResponse;

public class UserJoinResponse
{
    public string RoomId { get; set; }
    public UserInfo User { get; set; }
    public bool IsHost { get; set; }
}