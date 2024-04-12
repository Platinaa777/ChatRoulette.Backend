using Chat.Application.Models;
using Chat.Application.Queries.GetWinner;
using Chat.Domain.Errors;
using Chat.Domain.Repositories;
using DomainDriverDesignAbstractions;
using MediatR;

namespace Chat.Application.Commands.DefineWinner;

public class DefineWinnerQueryHandler 
    : IRequestHandler<DefineWinnerQuery, Result<WinnerInformation>>
{
    private readonly IRoundRepository _roundRepository;
    private readonly IChatUserRepository _chatUserRepository;

    public DefineWinnerQueryHandler(
        IRoundRepository roundRepository,
        IChatUserRepository chatUserRepository)
    {
        _roundRepository = roundRepository;
        _chatUserRepository = chatUserRepository;
    }
    
    public async Task<Result<WinnerInformation>> Handle(DefineWinnerQuery request, CancellationToken cancellationToken)
    {
        if (Guid.TryParse(request.RoundId, out var guidId))
        {
            return Result.Failure<WinnerInformation>(GameErrors.InvalidRoundId);
        }
        
        var round = await _roundRepository.FindById(guidId);
        if (round is null)
            return Result.Failure<WinnerInformation>(GameErrors.RoundNotFound);

        var winner = round.GetWinnerEmail();

        if (!round.IsHasCertainWinner())
        {
            return new WinnerInformation()
            {
                Winner = winner
            };
        }

        var chatUser1 = await _chatUserRepository.FindByEmail(round.FirstPlayerEmail);
        var chatUser2 = await _chatUserRepository.FindByEmail(round.SecondPlayerEmail);
        
        if (chatUser1 is null || chatUser2 is null)
            return Result.Failure<WinnerInformation>(ChatUserErrors.NotFound);

        if (chatUser1.Email == winner)
        {
            chatUser1.IncreasePoints();
            await _chatUserRepository.Update(chatUser1);
        } else if (chatUser1.Email == winner)
        {
            chatUser2.IncreasePoints();
            await _chatUserRepository.Update(chatUser2);
        }

        return new WinnerInformation() { Winner = winner };
    }
}