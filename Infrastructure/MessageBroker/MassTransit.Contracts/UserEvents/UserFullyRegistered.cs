namespace MassTransit.Contracts.UserEvents;

public class UserFullyRegistered
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string NickName { get; set; }
    public string[] Preferences { get; set; }
    public int Age { get; set; }

    public UserFullyRegistered(string id, string email, string nickName, string[] preferences, int age)
    {
        Id = id;
        Email = email;
        NickName = nickName;
        Preferences = preferences;
        Age = age;
    }
}