using WaitingRoom.Core.Models;
using WaitingRoom.Core.Repositories;

namespace WaitingRoom.Infrastructure.Repositories;

public class DialogRoomRepository : IDialogRoomRepository
{
    private List<TwoSeatsRoom> _rooms = new();
    
    public async Task<TwoSeatsRoom> CreateRoom()
    {
        var room = new TwoSeatsRoom()
        {
            Id = Guid.NewGuid().ToString()
        };
        
        _rooms.Add(room);

        return room;
    }

    public async Task<TwoSeatsRoom> JoinRoom(UserInfo user, string roomId)
    {
        foreach (var room in _rooms)
        {
            if (room.Id == roomId)
            {
                room.Talkers.Add(user);
                return room;
            }
        }

        return new TwoSeatsRoom();
    }

    public async Task<bool> LeaveRoom(string roomId, UserInfo user)
    {
        foreach (var room in _rooms)
        {
            if (room.Id == roomId)
            {
                room.Talkers.Remove(user);

                var roomShouldDelete 
                    = room.Talkers.Count == 0 && _rooms.Remove(room);
                return true;
            }
        }

        return false;
    }

    public async Task<TwoSeatsRoom?> CanConnectToAnyRoom()
    {
        foreach (var room in _rooms)
        {
            if (room.Talkers.Count == 1)
            {
                return room;
            }
        }

        return null;
    }

    public async Task<TwoSeatsRoom?> FindRoomById(string id)
    {
        foreach (var meeting in _rooms)
        {
            if (meeting.Id == id)
            {
                return meeting;
            }
        }

        return null;
    }

    public async Task<List<TwoSeatsRoom>> GetAllRooms()
    {
        return _rooms;
    }
}