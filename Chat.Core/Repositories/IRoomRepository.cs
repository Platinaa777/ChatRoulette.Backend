namespace Chat.Core.Repositories;

public interface IRoomRepository
{
    Task<string> GetFreeRoom();
    Task CreateRoom(string room);
    Task<bool> DisconnectFromRoom(string room);
    Task<bool> ConnectToRoom(string room);
    bool IsAllRoomsFull();
    bool NotAnyRoomToJoin();
}