using AuthService.Application.Models;
using AuthService.Domain.Shared;
using MediatR;

namespace AuthService.Application.Commands.GenerateToken;

public class GenerateTokenCommand : IRequest<Result<AuthTokens>>
{
    public string RefreshToken { get; set; }
}