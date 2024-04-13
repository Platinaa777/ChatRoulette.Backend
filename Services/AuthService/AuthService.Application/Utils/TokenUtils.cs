using AuthService.Application.Security;
using AuthService.Domain.Models.TokenAggregate;
using AuthService.Domain.Models.TokenAggregate.ValueObjects;
using AuthService.Domain.Models.UserAggregate;
using DomainDriverDesignAbstractions;

namespace AuthService.Application.Utils;

public static class TokenUtils
{
    public static Result<(string accessToken, RefreshToken refreshToken)> CreateAuthPair(
        IJwtManager jwtManager,
        User user)
    {
        var accessToken = jwtManager.GenerateAccessToken(user);
        var refreshTokenValue = jwtManager.GenerateRefreshToken();
        
        // creating refresh-token
        Result<RefreshToken> refreshTokenResult = RefreshToken.Create(
            id: Guid.NewGuid().ToString(),
            token: refreshTokenValue,
            expiredAt: DateTime.Now.AddDays(3),
            isUsed: false,
            userId: user.Id.Value);

        if (refreshTokenResult.IsFailure)
            return Result.Failure<(string, RefreshToken)>(refreshTokenResult.Error);

        return (accessToken, refreshTokenResult.Value);
    }
}