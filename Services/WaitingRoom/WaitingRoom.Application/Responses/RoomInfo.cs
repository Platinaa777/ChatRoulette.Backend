using Chat.Application.Requests;
using WaitingRoom.Core.Models;

namespace WaitingRoom.Application.Responses;

public class RoomInfo
{
    public string Id { get; set; }
    public UserInfo Host { get; set; }
    public UserInfo Participant { get; set; }
    public bool IsExist { get; set; }
}