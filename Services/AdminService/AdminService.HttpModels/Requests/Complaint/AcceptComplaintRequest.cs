namespace AdminService.HttpModels.Requests.Complaint;

public class AcceptComplaintRequest
{
    public string Id { get; set; }
    public int DurationMinutes { get; set; }
}