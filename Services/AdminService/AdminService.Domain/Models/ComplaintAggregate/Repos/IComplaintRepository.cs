using AdminService.Domain.Models.ComplaintAggregate.ValueObjects;

namespace AdminService.Domain.Models.ComplaintAggregate.Repos;

public interface IComplaintRepository
{
    Task<Complaint?> FindComplaintById(Id id);
    Task<Complaint?> FindComplaintByEmails(Email sender, Email violator);
    Task<List<Complaint>> GetComplaints(int count);
    Task AddComplaint(Complaint complaint);
    Task<bool> UpdateComplaint(Complaint complaint);
}