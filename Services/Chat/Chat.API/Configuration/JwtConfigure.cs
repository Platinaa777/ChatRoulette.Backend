using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Chat.API.Requests;
using Chat.API.Responses;
using Chat.Core.Secrets;
using Microsoft.IdentityModel.Tokens;

namespace Chat.API.Configuration;

public static class JwtConfigure
{
    public static string GenerateJwtToken(ZoomRequest body)
    {
        var securutyKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ZoomSecret.ClientSecret));
        var credentials = new SigningCredentials(securutyKey, SecurityAlgorithms.HmacSha256);
        
        // Calculate timestamps
        DateTime issuedAt = DateTime.UtcNow;
        DateTime expiration = issuedAt.AddHours(48); // Assuming 3 hours expiration, adjust as needed
        DateTime tokenExpiration = issuedAt.AddHours(48); // Assuming 3 hours expiration, adjust as needed
        
        // Convert timestamps to epoch format
        long expEpoch = new DateTimeOffset(expiration).ToUnixTimeSeconds();
        long iatEpoch = new DateTimeOffset(issuedAt).ToUnixTimeSeconds();
        long tokenExpEpoch = new DateTimeOffset(tokenExpiration).ToUnixTimeSeconds();

        
        var claims = new Claim[]
        {
            new Claim("appKey", ZoomSecret.ClientId),
            new Claim("sdkKey", ZoomSecret.ClientId),
            new Claim("role", body.role.ToString(), ClaimValueTypes.Integer64),
            new Claim("exp", expEpoch.ToString()),
            new Claim("iat", iatEpoch.ToString(), ClaimValueTypes.Integer64),
            new Claim("tokenExp", tokenExpEpoch.ToString()),
            new Claim("mn", body.meetingNumber, ClaimValueTypes.Integer64)
        };

        var token = new JwtSecurityToken(claims: claims, signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}