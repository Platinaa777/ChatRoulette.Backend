using Chat.Application.Models;

namespace WaitingRoom.HttpModels.HttpResponse;

public class RoomInfoResponse
{
    public string Id { get; set; }
    public UserInfo Host { get; set; }
    public UserInfo Participant { get; set; }
    public bool IsExist { get; set; }
}