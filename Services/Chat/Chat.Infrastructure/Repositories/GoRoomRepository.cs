using System.Collections.Concurrent;
using Chat.Core.Repositories;

namespace Chat.Infrastructure.Repositories;

public class GoRoomRepository : IRoomRepository
{
    private readonly ConcurrentDictionary<string, int> rooms = new();
    
    public Task<string> GetFreeRoom()
    {
        foreach (var nameRoom in rooms.Keys)
        {
            if (rooms[nameRoom] <= 1)
            {
                return Task.FromResult(nameRoom);
            }
        }

        return Task.FromResult("");
    }

    public Task<bool> DisconnectFromRoom(string room)
    {
        if (!rooms.ContainsKey(room))
        {
            return Task.FromResult(false);
        }

        if (rooms[room] > 0)
        {
            rooms[room]--;
        }

        return Task.FromResult(true);
    }

    public Task<bool> ConnectToRoom(string room)
    {
        if (rooms[room] >= 2)
        {
            return Task.FromResult(false);
        }

        rooms[room]++;

        return Task.FromResult(true);
    }

    public bool IsAllRoomsFull()
    {
        var isFull = rooms.Count == 10;

        if (!isFull)
        {
            return false;
        }
        
        foreach (var room in rooms.Keys)
        {
            if (rooms[room] < 2)
            {
                return false;
            }
        }

        return true;
    }

    public bool NotAnyRoomToJoin()
    {
        foreach (var room in rooms.Keys)
        {
            if (rooms[room] < 2)
            {
                return false;
            }
        }

        return true;
    }

    public Task CreateRoom(string room)
    {
        if (!rooms.ContainsKey(room))
        {
            rooms[room] = 0;
        }

        return Task.CompletedTask;
    }
    
    
}