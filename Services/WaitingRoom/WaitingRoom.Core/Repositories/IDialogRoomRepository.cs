
using WaitingRoom.Core.Models;

namespace WaitingRoom.Core.Repositories;

public interface IDialogRoomRepository
{
    Task<TwoSeatsRoom> CreateRoom();
    Task<TwoSeatsRoom> JoinRoom(ChatUser chatUser, string roomId);
    Task<TwoSeatsRoom?> LeaveRoom(string roomId, ChatUser chatUser);
    Task<TwoSeatsRoom?> CanConnectToAnyRoom();
    Task<TwoSeatsRoom?> FindRoomById(string id);
    Task<List<TwoSeatsRoom>> GetAllRooms();
}