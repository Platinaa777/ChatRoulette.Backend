namespace MassTransit.Contracts.ComplaintEvents;

public class ComplaintRegistered
{
    public string Id { get; set; }
    public string Content { get; set; }
    public string From { get; set; }
    public string PossibleIntruderEmail { get; set; }
}