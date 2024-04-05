namespace MassTransit.Contracts.ComplaintEvents;

public class ComplaintApprovedByAdmin
{
    public string Id { get; set; }
    public string SenderEmail { get; set; }
    public string ViolatorEmail { get; set; }
    public int MinutesToBan { get; set; }
    public string Type { get; set; }
}