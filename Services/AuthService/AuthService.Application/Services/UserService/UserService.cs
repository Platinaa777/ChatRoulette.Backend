// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;
// using AuthService.Domain.JwtConfig;
// using AuthService.Domain.Models;
// using AuthService.Domain.Models.UserAggregate.Entities;
// using AuthService.HttpModels.Requests;
// using AuthService.HttpModels.Responses;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.Extensions.Logging;
// using Microsoft.Extensions.Options;
// using Microsoft.IdentityModel.Tokens;
//
// namespace AuthService.Application.Services;
//
// public class UserService : IUserService
// {
//     private readonly UserManager<User> _userManager;
//     private readonly RoleManager<IdentityRole> _roleManager;
//     private readonly ILogger<UserService> _logger;
//     private readonly Jwt _jwt;
//     
//     public UserService(
//         UserManager<User> userManager,
//         RoleManager<IdentityRole> roleManager,
//         IOptions<Jwt> jwt,
//         ILogger<UserService> logger)
//     {
//         _userManager = userManager;
//         _roleManager = roleManager;
//         _logger = logger;
//         _jwt = jwt.Value;
//     }
//
//     public async Task<bool> RegisterAsync(RegisterRequest request)
//     {
//         var user = new User()
//         {
//             UserName = request.UserName,
//             Age = request.Age ?? 0,
//             Email = request.Email,
//         };
//
//         var isUserExist = await _userManager.FindByEmailAsync(request.Email);
//         if (isUserExist != null)
//             return false;
//
//         var createdUser = await _userManager.CreateAsync(user);
//         if (createdUser.Succeeded)
//         {
//             return true;
//         }
//
//         foreach (var error in createdUser.Errors)
//         {
//             _logger.LogError(error.Description);
//         }
//
//         return false;
//     }
//
//     public async Task<AuthenticationResponse> GetTokenAsync(TokenRequest request)
//     {
//         var isUserExist = await _userManager.FindByEmailAsync(request.Email);
//         if (isUserExist == null)
//             return new AuthenticationResponse() { IsAuthenticate = false };
//
//         var isCorrectPassword = await _userManager.CheckPasswordAsync(isUserExist, request.Password);
//
//         if (!isCorrectPassword)
//             return new AuthenticationResponse() { IsAuthenticate = false };
//
//         var role = await _userManager.GetRolesAsync(isUserExist);
//         // Create claims for the user
//         var claims = new[]
//         {
//             new Claim("name", isUserExist.UserName),
//             new Claim("email", isUserExist.Email),
//             new Claim(ClaimTypes.Role, role.FirstOrDefault())
//             // Add any additional claims as needed
//         };
//
//         // Generate signing credentials using a secret key
//         var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
//         var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
//
//         // Set token expiration time
//         var expiration = DateTime.UtcNow.AddMinutes(_jwt.ValidationMins);
//
//         // Create the JWT token
//         var token = new JwtSecurityToken(
//
//             claims: claims,
//             expires: expiration,
//             signingCredentials: credentials
//         );
//
//         // Serialize the token to a string
//         var tokenHandler = new JwtSecurityTokenHandler();
//         var tokenString = tokenHandler.WriteToken(token);
//
//         
//         var response = new AuthenticationResponse()
//         {
//             IsAuthenticate = true,
//             UserName = isUserExist.UserName,
//             Token = tokenString,
//             Role = role.FirstOrDefault()
//         };
//
//         return response;
//
//         
//     }
// }