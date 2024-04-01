namespace AdminService.DataContext.Outbox;

public class OutboxMessage
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime StartedAtUtc { get; set; }
    public DateTime? HandledAtUtc { get; set; }
    public string? Error { get; set; }
}