using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AuthService.Application.JwtConfig;
using AuthService.Application.Security;
using AuthService.Domain.Models.UserAggregate;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Infrastructure.JwtGenerator;

public class JwtTokenCreator : IJwtManager
{
    private readonly Jwt _jwt;

    public JwtTokenCreator(IOptions<Jwt> jwt)
    {
        _jwt = jwt.Value;
    }

    public string GenerateAccessToken(User user)
    {
        // Create claims for the user
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.UserName.Value),
            new Claim(ClaimTypes.Email, user.Email.Value),
            new Claim("Role", user.Role.Name)
        };

        // Generate signing credentials using a secret key
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Set token expiration time
        var expiration = DateTime.Now.AddMinutes(_jwt.ValidationMins);
        Console.WriteLine(expiration.ToLocalTime());
        // Create the JWT token
        var token = new JwtSecurityToken(
            claims: claims,
            expires: expiration,
            signingCredentials: credentials
        );

        // Serialize the token to a string
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenString = tokenHandler.WriteToken(token);

        return tokenString;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    public ClaimsPrincipal GetClaimsPrincipal(string jwtToken)
    {
        var tokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key))
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(
            jwtToken,
            tokenValidationParameters,
            out SecurityToken securityToken);
        
        JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
        
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }
        
        return principal;
    }
}