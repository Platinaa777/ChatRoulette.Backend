namespace WaitingRoom.Application.Responses;

public class RoomInfo
{
    public string Id { get; set; }
    public string HostUrl { get; set; }
    public string JoinUrl { get; set; }
    public bool IsExist { get; set; }
}