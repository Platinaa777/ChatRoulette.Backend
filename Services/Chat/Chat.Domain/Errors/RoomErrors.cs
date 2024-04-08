using DomainDriverDesignAbstractions;

namespace Chat.Domain.Errors;

public class RoomErrors : Error
{
    public static Error RoomNotFoundById = new("Room.Error", "Room not found by this id");
    public static Error RoomCantClose = new("Room.Error", "Room can not be closed");
    public static Error NotFullRoom = new("Room.Error", "Room was not be full");
    public static Error RoomNotFoundByParticipant = new("Room.Error", "Room not found by participant");

    public RoomErrors(string code, string message) : base(code, message)
    {
    }
}