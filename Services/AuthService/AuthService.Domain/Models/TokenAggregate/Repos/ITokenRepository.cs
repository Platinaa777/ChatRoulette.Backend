using AuthService.Domain.Models.TokenAggregate.ValueObjects.Token;

namespace AuthService.Domain.Models.TokenAggregate.Repos;

public interface ITokenRepository
{
    Task<bool> AddRefreshToken(RefreshToken token);
    Task<bool> UpdateRefreshToken(RefreshToken token);
    Task<RefreshToken?> GetRefreshTokenByValue(Token token);
    Task<RefreshToken?> FindValidRefreshTokenByUserId(UserId userId);
}