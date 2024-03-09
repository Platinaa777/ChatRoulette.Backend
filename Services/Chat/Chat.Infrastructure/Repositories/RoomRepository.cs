using System.Collections.Concurrent;
using Chat.Domain.Entities;
using Chat.Domain.Repositories;

namespace Chat.Infrastructure.Repositories;

public class RoomRepository : IRoomRepository
{
    private ConcurrentDictionary<string, TwoSeatsRoom> _rooms = new();
    private readonly object _locker = new object();
    
    public async Task<TwoSeatsRoom?> TryToConnectRoom(ChatUser chatUser)
    {
        foreach (var key in _rooms.Keys)
        {
            lock (_locker)
            {
                if (_rooms[key].Peers.Count == 1 && !_rooms[key].Peers[0]!.PreviousParticipantIds.Contains(chatUser.Id))
                {
                    _rooms[key].Peers.Add(chatUser);
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
        room.Peers.Add(chatUser);
        _rooms[room.Id] = room;
        return room;
    }

    public async Task<bool> CloseRoom(string roomId)
    {
        return _rooms.TryRemove(roomId, out var _);
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