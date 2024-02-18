using ProfileService.Application.Commands;
using ProfileService.HttpModels.Requests;

namespace ProfileService.Api.Mappers;

public static class UserProfileMapper
{
    public static AddUserProfileCommand ToAddCommand(this AddUserProfileRequest request)
        => new AddUserProfileCommand()
        {
            UserName = request.UserName,
            NickName = request.NickName,
            Email = request.Email
        };
}