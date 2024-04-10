using System.Collections.Concurrent;
using System.Data;
using Chat.DataContext.Database;
using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly ChatDbContext _dbContext;

    public RoomRepository(ChatDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<TwoSeatsRoom?> TryToConnectRoom(ChatUser chatUser)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        var rooms = await _dbContext.Rooms
            .Where(x => x.ClosedAt == null)
            .Take(ChatUser.MaxLenUserHistory + 5)
            .ToListAsync();

        foreach (var room in rooms)
        {
            if (room is { PeerLinks.Count: 1 } && !chatUser.CheckInHistory(room.PeerLinks[0].Email))
            {
                room.AddPeer(chatUser);
                _dbContext.Rooms.Update(room);
                await _dbContext.SaveChangesAsync();
            
                await transaction.CommitAsync();
                return room;
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