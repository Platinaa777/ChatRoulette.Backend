using AuthService.Application.Models;
using AuthService.Domain.Models.TokenAggregate;

namespace AuthService.Application.Mappers;

public static class TokenMapper
{
    public static AuthTokens ToResponse(this RefreshToken refreshToken, string accessToken) =>
        new AuthTokens()
        {
            RefreshToken = refreshToken.Token.Value,
            AccessToken = accessToken
        };
}