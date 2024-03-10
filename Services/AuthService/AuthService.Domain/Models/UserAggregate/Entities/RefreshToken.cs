using AuthService.Domain.Models.UserAggregate.ValueObjects.Token;
using AuthService.Domain.SeedWork;

namespace AuthService.Domain.Models.UserAggregate.Entities;

public class RefreshToken : Entity<string>
{
    public static RefreshToken Create(Guid id, string token, DateTime expiredAt, bool isUsed) => 
        new RefreshToken(
            id: id.ToString(),
            new Token(token),
            expiredAt,
            isUsed);
    
    private RefreshToken(
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

    private RefreshToken() {}
}