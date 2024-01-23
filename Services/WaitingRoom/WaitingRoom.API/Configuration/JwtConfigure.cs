using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WaitingRoom.API.Responses;
using Chat.Core.Secrets;
using Microsoft.IdentityModel.Tokens;
using WaitingRoom.API.Requests;

namespace WaitingRoom.API.Configuration;

public static class JwtConfigure
{
    public static string GenerateJwtToken(ZoomRequest body)
    {
        var securutyKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ZoomSecret.GeneralClientSecret));
        var credentials = new SigningCredentials(securutyKey, SecurityAlgorithms.HmacSha256);
        
        // Calculate timestamps
        DateTime iat = DateTime.Now;
        DateTime exp = iat.AddHours(46); // Assuming 3 hours expiration, adjust as needed
        DateTime tokenExp = iat.AddHours(46); // Assuming 3 hours expiration, adjust as needed
        
        // Convert timestamps to epoch format
        long expEpoch = new DateTimeOffset(exp).ToUnixTimeSeconds();
        long iatEpoch = new DateTimeOffset(iat).ToUnixTimeSeconds();
        long tokenExpEpoch = new DateTimeOffset(tokenExp).ToUnixTimeSeconds();

        
        var claims = new Claim[]
        {
            new Claim("appKey", ZoomSecret.GeneralClientId),
            new Claim("sdkKey", ZoomSecret.GeneralClientId),
            new Claim("mn", body.meetingNumber, ClaimValueTypes.Integer64),
            new Claim("role", body.role.ToString(), ClaimValueTypes.Integer64),
            new Claim("iat", iatEpoch.ToString(), ClaimValueTypes.Integer64),
            new Claim("exp", expEpoch.ToString(), ClaimValueTypes.Integer64),
            new Claim("tokenExp", tokenExpEpoch.ToString(), ClaimValueTypes.Integer64),
        };

        var token = new JwtSecurityToken(claims: claims, signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}