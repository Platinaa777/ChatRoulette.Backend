namespace MassTransit.Contracts.ComplaintEvents;

public class ComplaintRegistered
{
    public string Id { get; set; }
    public string Content { get; set; }
    public string SenderEmail { get; set; }
    public string PossibleViolatorEmail { get; set; }
    public string ComplaintType { get; set; }
}