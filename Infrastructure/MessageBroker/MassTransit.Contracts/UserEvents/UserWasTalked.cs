namespace MassTransit.Contracts.UserEvents;

public class UserWasTalked
{
    public string Email { get; set; }
    public int Minites { get; set; }

    public UserWasTalked(string email, int minites)
    {
        Email = email;
        Minites = minites;
    }
}