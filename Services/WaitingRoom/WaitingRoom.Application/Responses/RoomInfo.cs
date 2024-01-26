namespace WaitingRoom.Application.Responses;

public class RoomInfo
{
    public string Id { get; set; }
    public List<string> Listeners { get; set; }
    public bool IsExist { get; set; }
}