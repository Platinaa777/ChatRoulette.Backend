
using WaitingRoom.Core.Models;

namespace WaitingRoom.Core.Repositories;

public interface IDialogRoomRepository
{
    void CreateRoom(string roomId, string connectionString);
    string JoinRoom(string roomId, string userEmail);
    bool LeaveRoom(string roomId, string userEmail);
    public TwoSeatsRoom? CanConnectToAnyRoom();
    public TwoSeatsRoom FindRoomById(string id);
}