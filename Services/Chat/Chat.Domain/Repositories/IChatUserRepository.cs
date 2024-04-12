using Chat.Domain.Aggregates;
using Chat.Domain.Aggregates.Room;

namespace Chat.Domain.Repositories;

public interface IChatUserRepository
{
    Task<ChatUser?> FindByEmail(string email);
    Task Add(ChatUser chatUser);
    Task Update(ChatUser chatUser);
}