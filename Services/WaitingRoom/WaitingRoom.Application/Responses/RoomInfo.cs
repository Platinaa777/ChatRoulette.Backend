using Chat.Application.Requests;
using WaitingRoom.Core.Models;

namespace WaitingRoom.Application.Responses;

public class RoomInfo
{
    public string Id { get; set; }
    public ChatUser Host { get; set; }
    public ChatUser Participant { get; set; }
    public bool IsExist { get; set; }
}