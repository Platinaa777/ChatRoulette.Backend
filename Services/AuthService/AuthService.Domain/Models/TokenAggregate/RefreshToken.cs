using AuthService.Domain.Errors.TokenErrors;
using AuthService.Domain.Models.Shared;
using AuthService.Domain.Models.TokenAggregate.ValueObjects;
using AuthService.Domain.Models.UserAggregate.ValueObjects;
using DomainDriverDesignAbstractions;

namespace AuthService.Domain.Models.TokenAggregate;

public class RefreshToken : AggregateRoot<Id>
{
    private RefreshToken(
        Id id,
        Token token,
        DateTime expiredAt,
        bool isUsed,
        Id userId) : base(id)
    {
        Token = token;
        ExpiredAt = expiredAt.ToUniversalTime();
        IsUsed = isUsed;
        UserId = userId;
    }
    public Token Token { get; private set; }
    public DateTime ExpiredAt { get; private set; }
    public bool IsUsed { get; private set; }
    public Id UserId { get; init; }

    public void SetUsed()
    {
        IsUsed = true;
    }

    public bool IsExpired() => ExpiredAt > DateTime.Now;
    public bool WasUsed() => IsUsed;
    
    public static Result<RefreshToken> Create(string id, string token, DateTime expiredAt, bool isUsed, string userId)
    {
        var idResult = Shared.Id.CreateId(id);
        if (idResult.IsFailure)
            return Result.Failure<RefreshToken>(idResult.Error);
        
        var refreshToken = Token.Create(token);
        if (refreshToken.IsFailure)
            return Result.Failure<RefreshToken>(refreshToken.Error);
        
        var userIdResult = Id.CreateId(userId);
        if (userIdResult.IsFailure)
            return Result.Failure<RefreshToken>(userIdResult.Error);

        if (expiredAt < DateTime.Now)
            return Result.Failure<RefreshToken>(TokenError.InvalidExpiredTime);

        return new RefreshToken(
            idResult.Value,
            token: refreshToken.Value,
            expiredAt,
            isUsed,
            userId: userIdResult.Value);
    }

    private RefreshToken() {}
}