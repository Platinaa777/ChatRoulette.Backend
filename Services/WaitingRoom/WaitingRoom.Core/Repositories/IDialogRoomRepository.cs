
using WaitingRoom.Core.Models;

namespace WaitingRoom.Core.Repositories;

public interface IDialogRoomRepository
{
    Task<TwoSeatsRoom> CreateRoom();
    Task<TwoSeatsRoom> JoinRoom(UserInfo user, string roomId);
    Task<TwoSeatsRoom?> LeaveRoom(string roomId, UserInfo user);
    Task<TwoSeatsRoom?> CanConnectToAnyRoom();
    Task<TwoSeatsRoom?> FindRoomById(string id);
    Task<List<TwoSeatsRoom>> GetAllRooms();
}