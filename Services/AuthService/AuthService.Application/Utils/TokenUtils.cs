using AuthService.Application.Security;
using AuthService.Domain.Models.TokenAggregate;
using AuthService.Domain.Models.TokenAggregate.ValueObjects.Token;
using AuthService.Domain.Models.UserAggregate;
using AuthService.Domain.Shared;

namespace AuthService.Application.Utils;

public static class TokenUtils
{
    public static Result<(string accessToken, RefreshToken refreshToken)> CreateAuthPair(IJwtManager jwtManager, User user)
    {
        var accessToken = jwtManager.GenerateAccessToken(user);
        var refreshTokenValue = jwtManager.GenerateRefreshToken();
        
        // creating refresh-token
        Result<RefreshToken> refreshTokenResult = RefreshToken.Create(
            id: Guid.NewGuid(),
            token: refreshTokenValue,
            expiredAt: DateTime.Now.AddMinutes(1.5),
            isUsed: false,
            userId: user.Id);

        if (refreshTokenResult.IsFailure)
            return Result.Failure<(string, RefreshToken)>(refreshTokenResult.Error);

        return (accessToken, refreshTokenResult.Value);
    }
}