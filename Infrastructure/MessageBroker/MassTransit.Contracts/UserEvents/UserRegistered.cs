namespace MassTransit.Contracts.UserEvents;

public class UserRegistered
{
    public string Email { get; set; }
    public string UserName { get; set; }
    public string NickName { get; set; }
}