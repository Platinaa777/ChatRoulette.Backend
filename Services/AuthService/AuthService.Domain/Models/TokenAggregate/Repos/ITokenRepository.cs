using AuthService.Domain.Models.Shared;
using AuthService.Domain.Models.TokenAggregate.ValueObjects;
using AuthService.Domain.Models.UserAggregate.ValueObjects;

namespace AuthService.Domain.Models.TokenAggregate.Repos;

public interface ITokenRepository
{
    Task<bool> AddRefreshToken(RefreshToken token);
    Task<bool> UpdateRefreshToken(RefreshToken token);
    Task<RefreshToken?> GetRefreshTokenByValue(Token token);
    Task<RefreshToken?> FindValidRefreshTokenByUserId(Id userId);
}