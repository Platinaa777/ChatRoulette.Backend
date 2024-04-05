using ProfileService.Domain.Models.Identity;

namespace ProfileService.Domain.Models.UserHistoryAggregate.Repos;

public interface IUserHistoryRepository
{
    Task<UserHistory?> FindByUserId(Id id);
    Task<UserHistory?> FindByHistoryId(Id id);
    Task<bool> AddHistory(UserHistory history);
    Task<bool> UpdateHistory(UserHistory history);
}