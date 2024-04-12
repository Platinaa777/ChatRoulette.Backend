using Chat.Domain.Errors;
using Chat.Domain.Repositories;
using DomainDriverDesignAbstractions;
using MediatR;

namespace Chat.Application.Commands.ChooseAnswer;

public class ChooseAnswerCommandHandler
    : IRequestHandler<ChooseAnswerCommand, Result>
{
    private readonly IRoundRepository _roundRepository;

    public ChooseAnswerCommandHandler(
        IRoundRepository roundRepository)
    {
        _roundRepository = roundRepository;
    }
    
    public async Task<Result> Handle(ChooseAnswerCommand request, CancellationToken cancellationToken)
    {
        if (Guid.TryParse(request.RoundId, out var guidId))
        {
            return Result.Failure(GameErrors.InvalidRoundId);
        }
        
        var round = await _roundRepository.FindById(guidId);
        
        if (round is null)
            return Result.Failure(GameErrors.RoundNotFound);
        
        round.SetAnswer(request.PlayerEmail, request.Answer);
        await _roundRepository.Update(round);

        return Result.Success();
    }
}