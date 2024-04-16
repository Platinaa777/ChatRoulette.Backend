using Chat.Application.Models;
using Chat.Domain.Errors;
using Chat.Domain.Repositories;
using DomainDriverDesignAbstractions;
using MediatR;

namespace Chat.Application.Queries.GetRecentPeers;

public class GetRecentPeersQueryHandler
    : IRequestHandler<GetRecentPeersQuery, Result<List<RecentPeerInformation>>>
{
    private readonly IChatUserRepository _chatUserRepository;

    public GetRecentPeersQueryHandler(
        IChatUserRepository chatUserRepository)
    {
        _chatUserRepository = chatUserRepository;
    }
    
    public async Task<Result<List<RecentPeerInformation>>> Handle(GetRecentPeersQuery request, CancellationToken cancellationToken)
    {
        var chatUser = await _chatUserRepository.FindByEmail(request.Email);

        if (chatUser is null)
            return Result.Failure<List<RecentPeerInformation>>(ChatUserErrors.NotFound);

        return chatUser.PreviousParticipantEmails.Select(email => new RecentPeerInformation(email)).ToList();
    }
}