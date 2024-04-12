using Chat.Application.Models;
using DomainDriverDesignAbstractions;
using MediatR;

namespace Chat.Application.Queries.GetWinner;

public class DefineWinnerQuery : IRequest<Result<WinnerInformation>>
{
    public string RoundId { get; set; }
}