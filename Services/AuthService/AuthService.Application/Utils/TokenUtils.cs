using AuthService.Application.Security;
using AuthService.Domain.Models.TokenAggregate;
using AuthService.Domain.Models.TokenAggregate.ValueObjects.Token;
using AuthService.Domain.Models.UserAggregate;

namespace AuthService.Application.Utils;

public static class TokenUtils
{
    public static (string, RefreshToken) CreateAuthPair(IJwtManager jwtManager, User user)
    {
        var accessToken = jwtManager.GenerateAccessToken(user);
        var refreshTokenValue = jwtManager.GenerateRefreshToken();
        
        // creating refresh-token
        RefreshToken refreshToken = RefreshToken.Create(
            id: Guid.NewGuid(),
            token: refreshTokenValue,
            expiredAt: DateTime.Now.AddHours(3),
            isUsed: false,
            userId: user.Id);

        return (accessToken, refreshToken);
    }
}