namespace MassTransit.Contracts.UserEvents;

public class UserFullyRegistered
{
    public string Email { get; set; }
    public string NickName { get; set; }
    public string[] Preferences { get; set; }
    public int Age { get; set; }

    public UserFullyRegistered(string email, string nickName, string[] preferences, int age)
    {
        Email = email;
        NickName = nickName;
        Preferences = preferences;
        Age = age;
    }
}