using System.Collections.Concurrent;
using Chat.DataContext.Database;
using Chat.Domain.Entities;
using Chat.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Repositories;

public class ChatUserRepository : IChatUserRepository
{
    private readonly ChatDbContext _dbContext;

    public ChatUserRepository(ChatDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<ChatUser?> FindByEmail(string email)
    {
        var chatUser = await _dbContext.ChatUsers.FirstOrDefaultAsync(x => x.Email == email); 
        return chatUser;
    }

    public async Task Add(ChatUser chatUser)
    {
        await _dbContext.ChatUsers.AddAsync(chatUser);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Update(ChatUser chatUser)
    {
        await _dbContext.ChatUsers.Where(u => u.Email == chatUser.Email)
            .ExecuteUpdateAsync(entity => entity
                .SetProperty(email => email.PreviousParticipantEmails, chatUser.PreviousParticipantEmails)
                .SetProperty(r => r.ConnectionId, chatUser.ConnectionId));
        await _dbContext.SaveChangesAsync();
    }
}