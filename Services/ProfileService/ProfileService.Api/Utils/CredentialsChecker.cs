using System.Security.Claims;
using AuthService.Application.Security;
using AuthService.Infrastructure.JwtGenerator;

namespace ProfileService.Api.Utils;

public class CredentialsChecker
{
    private readonly JwtTokenCreator _jwtManager;

    public CredentialsChecker(
        JwtTokenCreator jwtManager)
    {
        _jwtManager = jwtManager;
    }
    
    public string? GetEmailFromJwtHeader(string? token)
    {
        if (token is null) return null;
        
        var principal = _jwtManager.GetClaimsPrincipal(token);

        return GetEmailFromClaimsPrincipal(principal);
    }
    
    private string? GetEmailFromClaimsPrincipal(ClaimsPrincipal principal)
    {
        var emailClaim = principal.FindFirst(ClaimTypes.Email);
        if (emailClaim is not null)
        {
            var email = emailClaim.Value;
            return email;
        }
        return null;
    }
}