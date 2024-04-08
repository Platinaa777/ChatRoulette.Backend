namespace Chat.Application.Models;

public class TwoSeatsRoomInformation
{
    public string RoomId { get; set; } = "";
    public ChatUserInformation? FirstUser { get; set; }
    public ChatUserInformation? SecondUser { get; set; }
}