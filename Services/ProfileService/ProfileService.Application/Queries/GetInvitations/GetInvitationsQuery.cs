using DomainDriverDesignAbstractions;
using MediatR;
using ProfileService.Application.Models;

namespace ProfileService.Application.Queries.GetInvitations;

public class GetInvitationsQuery : IRequest<Result<List<InvitationResponse>>>
{
    public GetInvitationsQuery(string email)
    {
        Email = email;
    }
    
    public string Email { get; set; }
}