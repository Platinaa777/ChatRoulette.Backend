namespace MassTransit.Contracts.UserEvents;

public class UserSubmittedEmail
{
    public UserSubmittedEmail(string email)
    {
        Email = email;
    }
    public string Id { get; set; }
    public string Email { get; set; }
}