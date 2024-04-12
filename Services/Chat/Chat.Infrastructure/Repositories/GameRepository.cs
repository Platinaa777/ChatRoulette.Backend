using Chat.DataContext.Database;
using Chat.Domain.Aggregates.Game;
using Chat.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Repositories;

public class GameRepository : IGameRepository
{
    private readonly ChatDbContext _dbContext;

    public GameRepository(ChatDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<TranslationGame>> GetGames()
    {
        return await _dbContext.Games.ToListAsync();
    }

    public async Task<TranslationGame?> FindGameById(Guid id)
    {
        return await _dbContext.Games.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<TranslationGame?> GetRandomGame()
    {
        var randomGame = await _dbContext.Games
            .OrderBy(_ => Guid.NewGuid())
            .FirstOrDefaultAsync();

        return randomGame;
    }
}