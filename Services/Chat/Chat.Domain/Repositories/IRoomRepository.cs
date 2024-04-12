

using Chat.Domain.Aggregates;
using Chat.Domain.Aggregates.Room;

namespace Chat.Domain.Repositories;

public interface IRoomRepository
{
    Task<TwoSeatsRoom?> TryToConnectRoom(ChatUser chatUser);
    Task<TwoSeatsRoom> CreateRoomWithConnect(ChatUser chatUser);
    Task<bool> CloseRoom(string roomId);
    Task<TwoSeatsRoom?> FindRoomById(string id);
    Task<List<TwoSeatsRoom>> GetAllRooms();
}