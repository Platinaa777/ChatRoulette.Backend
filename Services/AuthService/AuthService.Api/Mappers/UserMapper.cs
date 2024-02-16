using AuthService.Application.Queries;
using AuthService.HttpModels.Requests;

namespace AuthService.Api.Mappers;

public static class UserMapper
{
    public static GetUserQuery ToQuery(this GetUserDataRequest request)
        => new GetUserQuery()
        {
            Email = request.Email,
            Password = request.Password
        };
}