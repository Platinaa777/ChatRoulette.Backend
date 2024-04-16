using Chat.Application.Models;
using DomainDriverDesignAbstractions;
using MediatR;

namespace Chat.Application.Queries.GetRecentPeers;

public class GetRecentPeersQuery : IRequest<Result<List<RecentPeerInformation>>>
{
    public GetRecentPeersQuery(string email)
    {
        Email = email;
    }

    public string Email { get; set; }
}