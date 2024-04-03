using Chat.Domain.Entities;
using DomainDriverDesignAbstractions;

namespace Chat.Domain.ValueObjects;

public class UserLink : ValueObject
{
    public UserLink(string email, string connectionId)
    {
        Email = email;
        ConnectionId = connectionId;
    }

    public string Email { get; set; }
    public string ConnectionId { get; set; }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Email;
        yield return ConnectionId;
    }
}

public static class UserLinkExtension {
    public static UserLink ToUserLink(this ChatUser chatUser)
    {
        return new UserLink(chatUser.Email, chatUser.ConnectionId);
    }
}