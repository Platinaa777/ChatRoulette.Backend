using Chat.Domain.Entities;

namespace Chat.Domain.Repositories;

public interface IChatUserRepository
{
    Task<ChatUser?> FindByEmail(string email);
    Task Add(ChatUser chatUser);
    Task Update(ChatUser chatUser);
}