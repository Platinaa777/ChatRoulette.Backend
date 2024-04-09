using DomainDriverDesignAbstractions;
using MediatR;

namespace ProfileService.Application.Queries.GetUserProfile;

public class GetUserProfileQuery : IRequest<Result<ProfileResponse>>
{
    public GetUserProfileQuery(string email)
    {
        Email = email;
    }

    public string Email { get; set; }
}