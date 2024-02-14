using AuthService.Application.Commands;
using AuthService.HttpModels.Requests;

namespace AuthService.Api.Mappers;

public static class RegisterMapper
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
}