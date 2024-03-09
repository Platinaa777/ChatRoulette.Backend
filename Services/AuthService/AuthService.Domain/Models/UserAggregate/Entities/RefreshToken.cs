using AuthService.Domain.Models.UserAggregate.ValueObjects.Token;
using AuthService.Domain.SeedWork;

namespace AuthService.Domain.Models.UserAggregate.Entities;

public class RefreshToken : Entity<string>
{
    public RefreshToken(
        string id,
        Token token,
        DateTime expiredAt,
        bool isUsed) : base(id)
    {
        Token = token;
        ExpiredAt = expiredAt;
        IsUsed = isUsed;
    }
    public Token Token { get; set; }
    public DateTime ExpiredAt { get; set; }
    public bool IsUsed { get; set; }
}