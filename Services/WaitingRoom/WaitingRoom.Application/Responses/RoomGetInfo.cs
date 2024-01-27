using WaitingRoom.Core.Models;

namespace WaitingRoom.Application.Responses;

public class RoomGetInfo
{
    public string Id { get; set; }
    public UserInfo Host { get; set; }
    public UserInfo Participant { get; set; }
}