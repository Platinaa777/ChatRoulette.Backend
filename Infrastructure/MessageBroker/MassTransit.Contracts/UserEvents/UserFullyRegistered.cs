namespace MassTransit.Contracts.UserEvents;

public class UserFullyRegistered
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public DateTime BirthDateUtc { get; set; }

    public UserFullyRegistered(string id, string email, string userName, DateTime birthDateUtc)
    {
        Id = id;
        Email = email;
        BirthDateUtc = birthDateUtc;
        UserName = userName;
    }
}