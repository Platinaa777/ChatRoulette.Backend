using AuthService.DataContext.Database;
using AuthService.Domain.Models.Shared;
using AuthService.Domain.Models.UserHistoryAggregate;
using AuthService.Domain.Models.UserHistoryAggregate.Repos;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Repos;

public class UserHistoryRepository : IUserHistoryRepository
{
    private readonly UserDb _dbContext;

    public UserHistoryRepository(UserDb dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<History?> FindByUserId(Id id)
    {
        return await _dbContext.Histories.FirstOrDefaultAsync(x => x.UserId == id);
    }

    public async Task<History?> FindByHistoryId(Id id)
    {
        return await _dbContext.Histories.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> AddHistory(History history)
    {
        await _dbContext.Histories.AddAsync(history);
        return true;
    }

    public async Task<bool> UpdateHistory(History history)
    {
        var result = await _dbContext.Histories.Where(h => h.Id == history.Id)
            .ExecuteUpdateAsync(entity => entity
                .SetProperty(historyProp => historyProp.BannedTime, history.BannedTime));

        return result == 1;
    }
}