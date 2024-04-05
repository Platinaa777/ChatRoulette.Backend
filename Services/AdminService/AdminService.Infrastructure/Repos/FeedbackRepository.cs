using AdminService.Domain.Models.FeedbackAggregate;
using AdminService.Domain.Models.FeedbackAggregate.Repos;
using AdminService.Domain.Models.Shared;
using Microsoft.EntityFrameworkCore;

namespace AdminService.Infrastructure.Repos;

public class FeedbackRepository : IFeedbackRepository
{
    private readonly DataContext.Database.DataContext _dbContext;


    public FeedbackRepository(DataContext.Database.DataContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Feedback?> FindById(Id id)
    {
        return await _dbContext.Feedbacks.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> AddFeedback(Feedback feedback)
    {
        await _dbContext.Feedbacks.AddAsync(feedback);
        return true;
    }

    public async Task<List<Feedback>> GetFeedbacks(int count)
    {
        return await _dbContext.Feedbacks
            .Where(x => !x.IsWatched)
            .Take(count)
            .ToListAsync();
    }
}