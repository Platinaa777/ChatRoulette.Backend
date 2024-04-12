using Chat.Application.Models;
using DomainDriverDesignAbstractions;
using MediatR;

namespace Chat.Application.Commands.StartRound;

public class StartRoundCommand : IRequest<Result<GameInformation>>
{
    public string FirstEmail { get; set; }
    public string SecondEmail { get; set; }
}