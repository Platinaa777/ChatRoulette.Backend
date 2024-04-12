using DomainDriverDesignAbstractions;
using MediatR;

namespace Chat.Application.Commands.ChooseAnswer;

public class ChooseAnswerCommand : IRequest<Result>
{
    public string RoundId { get; set; }
    public string PlayerEmail { get; set; }
    public string Answer { get; set; }
}