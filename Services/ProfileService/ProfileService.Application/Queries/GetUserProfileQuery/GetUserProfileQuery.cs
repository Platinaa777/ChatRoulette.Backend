using DomainDriverDesignAbstractions;
using MediatR;

namespace ProfileService.Application.Queries.GetUserProfileQuery;

public class GetUserProfileQuery : IRequest<Result<ProfileResponse>>
{
    public string Email { get; set; }
}