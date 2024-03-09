using System.Collections.Concurrent;
using Chat.Domain.Entities;
using Chat.Domain.Repositories;

namespace Chat.Infrastructure.Repositories;

public class ChatUserRepository : IChatUserRepository
{
    private ConcurrentDictionary<string, ChatUser> Users { get; set; } = new();
    
    public async Task<ChatUser?> FindById(string id)
    {
        await Task.CompletedTask;
        Users.TryGetValue(id, out var chatUser);
        return chatUser;
    }

    public async Task Add(ChatUser chatUser)
    {
        await Task.CompletedTask;
        Users[chatUser.Id] = chatUser;
    }

    public async Task Update(ChatUser chatUser)
    {
        await Task.CompletedTask;
        Users[chatUser.Id] = chatUser;
    }
}