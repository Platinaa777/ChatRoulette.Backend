using Chat.Domain.Aggregates.Game;

namespace Chat.Domain.Repositories;

public interface IGameRepository
{
    Task<List<TranslationGame>> GetGames();
    Task<TranslationGame?> FindGameById(Guid id);
    Task<TranslationGame?> GetRandomGame();
}