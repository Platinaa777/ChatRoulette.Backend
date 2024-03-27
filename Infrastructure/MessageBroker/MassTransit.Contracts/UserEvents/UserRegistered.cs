namespace MassTransit.Contracts.UserEvents;

public class UserRegistered
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
}