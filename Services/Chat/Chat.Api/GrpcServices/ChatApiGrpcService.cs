using Chat.Application.Queries.GetRecentPeers;
using Chat.Grpc;
using Grpc.Core;
using MediatR;

namespace Chat.Api.GrpcServices;

public class ChatApiGrpcService : ChatApiGrpc.ChatApiGrpcBase
{
    private readonly IMediator _mediator;

    public ChatApiGrpcService(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    public override async Task<GetRecentUserPeersResponse> GetRecentUserPeers(GetRecentUserPeersRequest request, ServerCallContext context)
    {
        var result = await _mediator.Send(new GetRecentPeersQuery(request.Email));

        if (result.IsFailure)
            return new GetRecentUserPeersResponse();

        var list = new GetRecentUserPeersResponse();

        foreach (var peer in result.Value)
            list.RecentPeerEmails.Add(peer.Email);

        return list;
    }
}