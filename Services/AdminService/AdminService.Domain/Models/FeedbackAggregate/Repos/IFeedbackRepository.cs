using AdminService.Domain.Models.Shared;

namespace AdminService.Domain.Models.FeedbackAggregate.Repos;

public interface IFeedbackRepository
{
    Task<Feedback?> FindById(Id id);
    Task<bool> AddFeedback(Feedback feedback);
    Task<List<Feedback>> GetFeedbacks(int count);
    Task UpdateFeedback(Feedback feedback);
}