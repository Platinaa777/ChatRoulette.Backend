using Chat.DataContext.Database;
using Chat.Domain.Aggregates.Game;
using Chat.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Repositories;

public class RoundRepository : IRoundRepository
{
    private readonly ChatDbContext _dbContext;

    public RoundRepository(ChatDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Round?> FindById(Guid id)
    {
        return await _dbContext.Rounds.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task Update(Round round)
    {
        if (round.FirstPlayerAnswer is null && round.SecondPlayerAnswer is not null)
        {
            await _dbContext.Rounds.Where(x => x.Id == round.Id)
                .ExecuteUpdateAsync(entity => entity
                    .SetProperty(prop1 => prop1.SecondPlayerAnswer, round.SecondPlayerAnswer)
                    .SetProperty(prop2 => prop2.SecondPlayerAnswerTime, round.SecondPlayerAnswerTime));
            return;
        }
        
        if (round.SecondPlayerAnswer is null && round.FirstPlayerAnswer is not null)
        {
            await _dbContext.Rounds.Where(x => x.Id == round.Id)
                .ExecuteUpdateAsync(entity => entity
                    .SetProperty(prop1 => prop1.FirstPlayerAnswer, round.FirstPlayerAnswer)
                    .SetProperty(prop2 => prop2.FirstPlayerAnswer, round.FirstPlayerAnswer));    
            return;
        }
        
        await _dbContext.Rounds.Where(x => x.Id == round.Id)
            .ExecuteUpdateAsync(entity => entity
                .SetProperty(prop1 => prop1.FirstPlayerAnswer, round.FirstPlayerAnswer)
                .SetProperty(prop2 => prop2.FirstPlayerAnswer, round.FirstPlayerAnswer)
                .SetProperty(prop1 => prop1.SecondPlayerAnswer, round.SecondPlayerAnswer)
                .SetProperty(prop2 => prop2.SecondPlayerAnswerTime, round.SecondPlayerAnswerTime));
    }

    public async Task Add(Round round)
    {
        await _dbContext.Rounds.AddAsync(round);
    }
}