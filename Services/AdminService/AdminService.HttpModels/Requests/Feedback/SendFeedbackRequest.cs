namespace AdminService.HttpModels.Requests.Feedback;

public class SendFeedbackRequest
{
    public string FromEmail { get; set; }
    public string Content { get; set; }
}