namespace MassTransit.Contracts.UserEvents;

public class UserSubmittedEmail
{
    public string UserName { get; set; }
    public string Email { get; set; }
}