using AdminService.Domain.Models.ComplaintAggregate;
using AdminService.Domain.Models.ComplaintAggregate.Repos;
using AdminService.Domain.Models.ComplaintAggregate.ValueObjects;
using AdminService.Domain.Models.Shared;
using Microsoft.EntityFrameworkCore;

namespace AdminService.Infrastructure.Repos;

public class ComplaintRepository : IComplaintRepository
{
    private readonly DataContext.Database.DataContext _dbContext;


    public ComplaintRepository(DataContext.Database.DataContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Complaint?> FindComplaintById(Id id)
    {
        var complaint = await _dbContext.Complaints.FirstOrDefaultAsync(x => x.Id == id);

        return complaint;
    }

    public async Task<Complaint?> FindComplaintByEmails(Email sender, Email violator)
    {
        var complaint = await _dbContext.Complaints.FirstOrDefaultAsync(x =>
            x.SenderEmail == sender && x.ViolatorEmail == violator);

        return complaint;
    }

    public async Task<List<Complaint>> GetComplaints(int count)
    {
        var complaints = await _dbContext.Complaints
            .Where(x => !x.IsHandled)
            .Take(count)
            .ToListAsync();

        return complaints;
    }

    public async Task AddComplaint(Complaint complaint)
    {
        await _dbContext.Complaints.AddAsync(complaint);
    }

    public async Task<bool> UpdateComplaint(Complaint complaint)
    {
        var result = await _dbContext.Complaints.Where(u => u.Id == complaint.Id)
            .ExecuteUpdateAsync(entity => entity
                .SetProperty(email => email.IsHandled, complaint.IsHandled)
                .SetProperty(r => r.ComplaintType, complaint.ComplaintType));

        return result == 1;
    }
}