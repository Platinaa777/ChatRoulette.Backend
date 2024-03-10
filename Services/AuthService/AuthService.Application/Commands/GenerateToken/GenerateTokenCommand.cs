using AuthService.Application.Models;
using MediatR;

namespace AuthService.Application.Commands.GenerateToken;

public class GenerateTokenCommand : IRequest<AuthTokens?>
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}