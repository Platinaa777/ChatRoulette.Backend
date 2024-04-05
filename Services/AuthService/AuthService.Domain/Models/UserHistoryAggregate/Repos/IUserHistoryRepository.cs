using AuthService.Domain.Models.Shared;

namespace AuthService.Domain.Models.UserHistoryAggregate.Repos;

public interface IUserHistoryRepository
{
    Task<History?> FindByUserId(Id id);
    Task<History?> FindByHistoryId(Id id);
    Task<bool> AddHistory(History history);
    Task<bool> UpdateHistory(History history);
}