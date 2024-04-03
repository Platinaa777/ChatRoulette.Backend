using System.Collections.Concurrent;
using Chat.DataContext.Database;
using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly ChatDbContext _dbContext;
    private readonly object _locker = new object();

    public RoomRepository(ChatDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<TwoSeatsRoom?> TryToConnectRoom(ChatUser chatUser)
    {
        foreach (var room in _dbContext.Rooms)
        {
            lock (_locker)
            {
                if (room.PeerEmails.Count == 1 && !chatUser.CheckInHistory(room.PeerEmails[0]))
                {
                    room.AddPeer(chatUser.Id);
                    return room;
                }
            }
        }

        return null;
    }
    
    public async Task<TwoSeatsRoom> CreateRoomWithConnect(ChatUser chatUser)
    {
        var room = new TwoSeatsRoom(
            id: Guid.NewGuid().ToString(),
            new List<string>() { chatUser.Email },
            DateTime.UtcNow);

        await _dbContext.Rooms.AddAsync(room);
        return room;
    }

    public async Task<bool> CloseRoom(string roomId)
    {
        var room = await _dbContext.Rooms.FirstOrDefaultAsync(x => x.Id == roomId);
        if (room is null)
            return false;

        room.Close();

        return true;
    }
    
    public async Task<TwoSeatsRoom?> FindRoomById(string id)
    {
        return await _dbContext.Rooms.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<TwoSeatsRoom>> GetAllRooms()
    {
        return _dbContext.Rooms.ToList();
    }
}