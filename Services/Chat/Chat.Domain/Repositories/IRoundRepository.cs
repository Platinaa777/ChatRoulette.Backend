using Chat.Domain.Aggregates.Game;

namespace Chat.Domain.Repositories;

public interface IRoundRepository
{
    Task<Round?> FindById(Guid id);
    Task Update(Round round);
    Task Add(Round round);
}