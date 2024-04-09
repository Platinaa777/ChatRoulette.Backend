using DomainDriverDesignAbstractions;

namespace AdminService.Domain.Errors;

public class ComplaintError : Error
{
    public static Error InvalidId = new("Complaint.Error", "Invalid value of id");
    public static Error EmptyContentMessage = new("Complaint.Error", "Content of complaint must be has text");
    public static Error InvalidEmail = new("Complaint.Error", "Invalid text of email");
    public static Error InvalidComplaintType = new("Complaint.Error", "This type of complaint does not exist");
    public static Error NotFoundAnyComplaints = new("Complaint.Error", "No stored complaints");
    public static Error ComplaintNotFound = new("Complaint.Error", "Complaint not found");
    public static Error CantUpdateComplaint = new("Complaint.Error", "Complaint cant be updated");
    public static Error AlreadyHandled = new("Complaint.Error", "Complaint already handled");

    
    public ComplaintError(string code, string message) : base(code, message)
    {
    }
}