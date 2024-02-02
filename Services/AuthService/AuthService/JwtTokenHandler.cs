using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.Models;
using AuthService.Requests;
using AuthService.Responses;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace AuthService;

public class JwtTokenHandler
{
    public const string KEY = "1234567890ABCDEF1234567890ABCDEF";
    public const int VALIDATION_MINS = 30;
    private readonly List<User> _users;

    public JwtTokenHandler()
    {
        _users = new()
        {
            new User() {Name = "admin", Password = "admin123", Role = Role.ADMIN},
            new User() {Name = "user", Password = "user123", Role = Role.ACTIVATED_USER}
        };
    }

    public UserAuthResponse? GenerateJwtToken(UserAuthRequest request)
    {
        if (string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Password))
        {
            return null;
        }

        var account = _users.FirstOrDefault(user => user.Name == request.Name && user.Password == request.Password);
        if (account == null) return null;

        var securityKey = Encoding.UTF8.GetBytes(KEY);
        var expiry = DateTime.UtcNow.AddMinutes(VALIDATION_MINS);
        var claims = new ClaimsIdentity(
            new List<Claim>()
            {
                new Claim("Name", request.Name),
                new Claim("Role", account.Role.ToString()) 
            });
        

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(securityKey),
            SecurityAlgorithms.HmacSha256Signature);
        var descriptor = new SecurityTokenDescriptor()
        {
            SigningCredentials = credentials,
            Expires = expiry,
            Subject = claims
        };

        var securityHandler = new JwtSecurityTokenHandler();
        var createdToken = securityHandler.CreateToken(descriptor);
        var token = securityHandler.WriteToken(createdToken);

        return new UserAuthResponse()
        {
            ExpiredAt = expiry.Second,
            Name = request.Name,
            Token = token
        };
    }
}