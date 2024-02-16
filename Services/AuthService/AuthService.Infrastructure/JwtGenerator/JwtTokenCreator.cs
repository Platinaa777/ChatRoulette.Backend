using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.Domain.JwtConfig;
using AuthService.Domain.Models.UserAggregate.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Infrastructure.JwtGenerator;

public class JwtTokenCreator
{
    private readonly Jwt _jwt;

    public JwtTokenCreator(IOptions<Jwt> jwt)
    {
        _jwt = jwt.Value;
    }
    
    public string CreateToken(User user)
    {
         // Create claims for the user
         var claims = new[]
         {
             new Claim("name", user.UserName.Value),
             new Claim("email", user.Email.Value),
             new Claim(ClaimTypes.Role, user.Role.Name)
         };

         // Generate signing credentials using a secret key
         var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
         var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

         // Set token expiration time
         var expiration = DateTime.UtcNow.AddMinutes(_jwt.ValidationMins);

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
}