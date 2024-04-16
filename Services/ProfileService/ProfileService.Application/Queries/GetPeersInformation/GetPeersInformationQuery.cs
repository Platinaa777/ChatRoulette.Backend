using DomainDriverDesignAbstractions;
using MediatR;
using ProfileService.Application.Models;

namespace ProfileService.Application.Queries.GetPeersInformation;

public class GetPeersInformationQuery
    : IRequest<Result<List<RecentPeerInformation>>>
{
    public List<string> PeerEmails { get; set; } = new();
}