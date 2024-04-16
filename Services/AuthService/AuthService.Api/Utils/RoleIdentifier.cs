using System.Security.Claims;
using AuthService.Application.Security;
using AuthService.Infrastructure.JwtGenerator;

namespace AuthService.Api.Utils;

public class RoleIdentifier
{
    private readonly IJwtManager _jwtManager;

    public RoleIdentifier(
        IJwtManager jwtManager)
    {
        _jwtManager = jwtManager;
    }
    
    public string? GetRoleFromJwtHeader(string? token)
    {
        if (token is null) return null;
        
        var principal = _jwtManager.GetClaimsPrincipal(token);

        return GetRoleFromClaimsPrincipal(principal);
    }
    
    private string? GetRoleFromClaimsPrincipal(ClaimsPrincipal principal)
    {
        var emailClaim = principal.FindFirst("Role");
        if (emailClaim is not null)
        {
            var email = emailClaim.Value;
            return email;
        }
        return null;
    }
}