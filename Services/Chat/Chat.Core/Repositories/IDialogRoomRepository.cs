using Chat.Core.Models;

namespace Chat.Core.Repositories;

public interface IDialogRoomRepository
{
    string CreateRoom(string id);
    string JoinRoom();
    string LeaveRoom();
    public TwoSeatsRoom? CanConnectToAnyRoom();
}