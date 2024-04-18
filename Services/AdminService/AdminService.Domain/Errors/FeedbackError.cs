using DomainDriverDesignAbstractions;

namespace AdminService.Domain.Errors;

public class FeedbackError : Error
{
    public static Error InvalidId = new("Feedback.Error", "Invalid value of id");
    public static Error NotEnoughLength = new("Feedback.Error", "The length of feedback should be greater that 10");
    public static Error InvalidEmail = new("Feedback.Error", "Invalid email");
    public static Error NotFound = new("Feedback.Error", "Feedback not found");
    
    public FeedbackError(string code, string message) : base(code, message)
    {
    }
}