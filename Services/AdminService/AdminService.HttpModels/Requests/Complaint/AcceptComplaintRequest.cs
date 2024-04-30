namespace AdminService.HttpModels.Requests.Complaint;

public class AcceptComplaintRequest
{
    public string Id { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
}