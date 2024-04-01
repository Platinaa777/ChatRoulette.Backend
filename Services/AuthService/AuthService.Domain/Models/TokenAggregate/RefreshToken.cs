using AuthService.Domain.Errors.TokenErrors;
using AuthService.Domain.Models.TokenAggregate.ValueObjects;
using DomainDriverDesignAbstractions;

namespace AuthService.Domain.Models.TokenAggregate;

public class RefreshToken : AggregateRoot<string>
{
    public static Result<RefreshToken> Create(Guid id, string token, DateTime expiredAt, bool isUsed, string userId)
    {
        var refreshToken = Token.Create(token);
        if (refreshToken.IsFailure)
            return Result.Failure<RefreshToken>(refreshToken.Error);
        
        var userIdResult = UserId.CreateId(userId);
        if (userIdResult.IsFailure)
            return Result.Failure<RefreshToken>(userIdResult.Error);

        if (expiredAt < DateTime.Now)
            return Result.Failure<RefreshToken>(TokenError.InvalidExpiredTime);

        return new RefreshToken(
            id: id.ToString(),
            token: refreshToken.Value,
            expiredAt,
            isUsed,
            userId: userIdResult.Value);
    }
    
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

    public bool IsExpired() => ExpiredAt > DateTime.Now;
    public bool WasUsed() => IsUsed;

    private RefreshToken() {}
}