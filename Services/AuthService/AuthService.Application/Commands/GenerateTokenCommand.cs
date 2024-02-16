using AuthService.HttpModels.Responses;
using MediatR;

namespace AuthService.Application.Commands;

public class GenerateTokenCommand : IRequest<AuthenticationResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }
}