using AuthService.HttpModels.Responses;
using MediatR;

namespace AuthService.Application.Queries;

public class GetUserQuery : IRequest<UserInformationResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }
}