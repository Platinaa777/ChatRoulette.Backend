using Chat.Core.Models;

namespace Chat.Core.Repositories;

public interface IDialogRoomRepository
{
    string CreateRoom(string roomId, string connectionString);
    string JoinRoom(string roomId, string userEmail);
    bool LeaveRoom(string roomId, string userEmail);
    public TwoSeatsRoom? CanConnectToAnyRoom();
}