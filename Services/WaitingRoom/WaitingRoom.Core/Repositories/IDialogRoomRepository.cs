
using WaitingRoom.Core.Models;

namespace WaitingRoom.Core.Repositories;

public interface IDialogRoomRepository
{
    void CreateRoom(string roomId);
    string JoinRoom(string use);
    bool LeaveRoom(string roomId, UserInfo user);
    public TwoSeatsRoom? CanConnectToAnyRoom();
    public TwoSeatsRoom FindRoomById(string id);
}