using System.Collections.Concurrent;
using Chat.Domain.Entities;
using Chat.Domain.Repositories;

namespace Chat.Infrastructure.Repositories;

public class RoomRepository : IRoomRepository
{
    private ConcurrentDictionary<string, TwoSeatsRoom> _rooms = new();
    private object locker = new object();
    
    public async Task<TwoSeatsRoom?> TryToConnectRoom(ChatUser chatUser)
    {
        // user can be store in some room already
        foreach (var key in _rooms.Keys)
        {
            if (_rooms[key].peers.Contains(chatUser))
            {
                return null;
            }
        }

        foreach (var key in _rooms.Keys)
        {
            lock (locker)
            {
                if (_rooms[key].peers.Count == 1)
                {
                    _rooms[key].peers.Add(chatUser);
                    return _rooms[key];
                }
            }
        }

        return null;
    }
    
    public async Task<TwoSeatsRoom> CreateRoomWithConnect(ChatUser chatUser)
    {
        // generate room with guid id
        var room = new TwoSeatsRoom();
        room.peers.Add(chatUser);
        _rooms[room.Id] = room;
        return room;
    }

    public async Task<TwoSeatsRoom?> LeaveRoom(string roomId, ChatUser chatUser)
    {
        foreach (var key in _rooms.Keys)
        {
            if (key == roomId)
            {
                _rooms[key].peers.Remove(chatUser);
                return _rooms[key];
            }
        }

        return null;
    }
    
    public async Task<TwoSeatsRoom?> FindRoomById(string id)
    {
        foreach (var key in _rooms.Keys)
        {
            if (key == id)
            {
                return _rooms[key];
            }
        }

        return null;
    }

    public async Task<List<TwoSeatsRoom>> GetAllRooms()
    {
        return _rooms.Select(x => x.Value).ToList();
    }
}