using Chat.Domain.Entities;

namespace Chat.Domain.Repositories;

public interface IChatUserRepository
{
    Task<ChatUser?> FindById(string id);
    Task Add(ChatUser chatUser);
    Task Update(ChatUser chatUser);
}