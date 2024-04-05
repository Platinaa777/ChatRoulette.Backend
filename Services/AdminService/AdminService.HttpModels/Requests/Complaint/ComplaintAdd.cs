namespace AdminService.HttpModels.Requests.Complaint;

public class ComplaintAdd
{
    public string Content { get; set; }
    public string SenderEmail { get; set; }
    public string PossibleViolatorEmail { get; set; }
    public string ComplaintType { get; set; }
}