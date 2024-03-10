using AuthService.Domain.Models.UserAggregate.Entities;
using AuthService.Domain.Models.UserAggregate.ValueObjects.Token;

namespace AuthService.Domain.Models.UserAggregate.Repos;

public interface ITokenRepository
{
    Task<bool> AddRefreshToken(RefreshToken token);
    Task<bool> UpdateRefreshToken(RefreshToken token);
    Task<RefreshToken?> GetRefreshTokenByValue(Token token);
}