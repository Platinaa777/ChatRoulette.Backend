namespace AdminService.Application.Models;

public class ComplaintInformation
{
    public string Id { get; set; }
    public string Content { get; set; }
    public string SenderEmail { get; set; }
    public string ViolatorEmail { get; set; }
    public string ComplaintType { get; set; }
}