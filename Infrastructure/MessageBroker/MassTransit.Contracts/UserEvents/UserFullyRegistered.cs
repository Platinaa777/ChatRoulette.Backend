namespace MassTransit.Contracts.UserEvents;

public class UserFullyRegistered
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string NickName { get; set; }
    public int Age { get; set; }

    public UserFullyRegistered(string id, string email, string nickName, int age)
    {
        Id = id;
        Email = email;
        NickName = nickName;
        Age = age;
    }
}