using AuthService.Application.Commands.CreateUser;
using AuthService.Application.Commands.GenerateToken;
using AuthService.Application.Commands.LoginUser;
using AuthService.Application.Queries.GetUser;
using AuthService.HttpModels.Requests;

namespace AuthService.Api.Mappers;

public static class DefaultMapper
{
    public static CreateUserCommand ToCommand(this RegisterRequest request)
    {
        return new CreateUserCommand()
        {
            UserName = request.UserName,
            NickName = request.NickName,
            Age = request.Age ?? -1,
            Email = request.Email,
            Password = request.Password,
        };
    }
    
    public static GenerateTokenCommand ToCommand(this TokenRequest request) =>
        new GenerateTokenCommand()
        {
            RefreshToken = request.RefreshToken,
        };
    
    public static GetUserQuery ToQuery(this GetUserDataRequest request)
        => new GetUserQuery()
        {
            Email = request.Email,
            Password = request.Password
        };

    public static LoginUserCommand ToCommand(this LoginRequest request) =>
        new LoginUserCommand()
        {
            Email = request.Email,
            Password = request.Password
        };
}