using System.Collections.Concurrent;
using Chat.DataContext.Database;
using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
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
        var rooms = await _dbContext.Rooms
            .Where(x => x.ClosedAt == null)
            .ToListAsync();
        
        foreach (var room in rooms)
        {
            lock (_locker)
            {
                if (room.PeerLinks.Count == 1 && !chatUser.CheckInHistory(chatUser.Email))
                {
                    room.AddPeer(chatUser);
                    _dbContext.Rooms.Update(room);
                    _dbContext.SaveChanges();
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
            new List<UserLink>() { chatUser.ToUserLink() },
            DateTime.UtcNow);

        await _dbContext.Rooms.AddAsync(room);
        await _dbContext.SaveChangesAsync();
        return room;
    }

    public async Task<bool> CloseRoom(string roomId)
    {
        var room = await _dbContext.Rooms.FirstOrDefaultAsync(x => x.Id == roomId);
        if (room is null)
            return false;

        room.Close();
        await _dbContext.SaveChangesAsync();
        return true;
    }
    
    public async Task<TwoSeatsRoom?> FindRoomById(string id)
    {
        return await _dbContext.Rooms.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<TwoSeatsRoom>> GetAllRooms()
    {
        return await _dbContext.Rooms.ToListAsync();
    }
}