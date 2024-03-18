using AuthService.Domain.Models.TokenAggregate.ValueObjects.Token;
using AuthService.Domain.SeedWork;

namespace AuthService.Domain.Models.TokenAggregate;

public class RefreshToken : Entity<string>
{
    public static RefreshToken Create(Guid id, string token, DateTime expiredAt, bool isUsed, string userId) => 
        new RefreshToken(
            id: id.ToString(),
            new Token(token),
            expiredAt,
            isUsed,
            UserId.CreateId(userId));
    
    private RefreshToken(
        string id,
        Token token,
        DateTime expiredAt,
        bool isUsed,
        UserId userId) : base(id)
    {
        Token = token;
        ExpiredAt = expiredAt.ToUniversalTime();
        IsUsed = isUsed;
        UserId = userId;
    }
    public Token Token { get; private set; }
    public DateTime ExpiredAt { get; private set; }
    public bool IsUsed { get; private set; }
    public UserId UserId { get; init; }

    public void SetUsed()
    {
        IsUsed = true;
    }

    private RefreshToken() {}
}