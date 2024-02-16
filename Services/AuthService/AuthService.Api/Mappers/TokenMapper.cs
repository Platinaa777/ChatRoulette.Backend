using AuthService.Application.Commands;
using AuthService.HttpModels.Requests;

namespace AuthService.Api.Mappers;

public static class TokenMapper
{
    public static GenerateTokenCommand ToCommand(this TokenRequest request) =>
        new GenerateTokenCommand()
        {
            Email = request.Email,
            Password = request.Password
        };
}