using WaitingRoom.Core.Models;
using WaitingRoom.Core.Repositories;

namespace WaitingRoom.Infrastructure.Repositories;

public class DialogRoomRepository : IDialogRoomRepository
{
    private List<TwoSeatsRoom> _rooms = new();
    
    public async Task<TwoSeatsRoom?> CanConnectToAnyRoom()
    {
        foreach (var room in _rooms)
        {
            if (room.Participant == null)
            {
                return room;
            }
        }

        return null;
    }
    
    public async Task<TwoSeatsRoom> CreateRoom()
    {
        // generate room with guid id
        var room = new TwoSeatsRoom();
        _rooms.Add(room);
        return room;
    }

    public async Task<TwoSeatsRoom> JoinRoom(ChatUser chatUser, string roomId)
    {
        foreach (var room in _rooms)
        {
            if (room.Id == roomId)
            {
                if (room.Host == null)
                {
                    room.Host = chatUser;
                }
                else
                {
                    room.Participant = chatUser;
                }
                return room;
            }
        }

        return new TwoSeatsRoom();
    }

    public async Task<TwoSeatsRoom?> LeaveRoom(string roomId, ChatUser chatUser)
    {
        foreach (var room in _rooms)
        {
            if (room.Id == roomId)
            {
                _rooms.Remove(room);
                return room;
            }
        }

        return null;
    }

    

    public async Task<TwoSeatsRoom?> FindRoomById(string id)
    {
        Console.WriteLine($"request id METHOD:[FindRoomById] = {id}");
        foreach (var meeting in _rooms)
        {
            Console.WriteLine($"room id (repo level) = {meeting.Id}");
            if (meeting.Id == id)
            {
                Console.WriteLine("yes (found)");
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