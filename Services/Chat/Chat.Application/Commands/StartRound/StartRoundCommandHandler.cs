using Chat.Application.Models;
using Chat.Domain.Aggregates.Game;
using Chat.Domain.Errors;
using Chat.Domain.Repositories;
using DomainDriverDesignAbstractions;
using MediatR;

namespace Chat.Application.Commands.StartRound;

public class StartRoundCommandHandler
    : IRequestHandler<StartRoundCommand, Result<GameInformation>>
{
    private readonly IGameRepository _gameRepository;
    private readonly IRoundRepository _roundRepository;

    public StartRoundCommandHandler(
        IGameRepository gameRepository,
        IRoundRepository roundRepository)
    {
        _gameRepository = gameRepository;
        _roundRepository = roundRepository;
    }
    
    public async Task<Result<GameInformation>> Handle(StartRoundCommand request, CancellationToken cancellationToken)
    {
        var randomGame = await _gameRepository.GetRandomGame();
        
        if (randomGame is null)
            return Result.Failure<GameInformation>(GameErrors.ZeroCountGames);

        Round round = new(
            id: Guid.NewGuid(),
            correctWord: randomGame.CorrectTranslation,
            firstPlayerEmail: request.FirstEmail,
            secondPlayerEmail: request.SecondEmail);

        await _roundRepository.Add(round);

        return new GameInformation()
        {
            Word = randomGame.WordToTranslate,
            ListTranslates = randomGame.ChooseList,
            RoundId = round.Id.ToString()
        };
    }
}