using AuthService.Domain.Shared;
using AuthService.HttpModels.Responses;
using MediatR;

namespace AuthService.Application.Queries.GetUser;

public class GetUserQuery : IRequest<Result<UserInformationResponse>>
{
    public string Email { get; set; }
    public string Password { get; set; }
}