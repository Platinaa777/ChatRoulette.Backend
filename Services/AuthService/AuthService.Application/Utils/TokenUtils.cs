using AuthService.Application.Security;
using AuthService.Domain.Models.UserAggregate.Entities;

namespace AuthService.Application.Utils;

public static class TokenUtils
{
    public static (string, RefreshToken) CreateAuthPair(IJwtManager jwtManager, User user)
    {
        var accessToken = jwtManager.GenerateAccessToken(user);
        var refreshTokenValue = jwtManager.GenerateRefreshToken();
        
        // creating refresh-token
        RefreshToken refreshToken = RefreshToken.Create(Guid.NewGuid(), refreshTokenValue,
            expiredAt: DateTime.Now.AddHours(3), isUsed: false);

        return (accessToken, refreshToken);
    }
}