using System.Security.Claims;
using AuthService.Domain.JwtConfig;
using AuthService.Domain.Models.UserAggregate.Entities;

namespace AuthService.Application.Security;

public interface IJwtManager
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal GetClaimsPrincipal(string jwtToken);
}