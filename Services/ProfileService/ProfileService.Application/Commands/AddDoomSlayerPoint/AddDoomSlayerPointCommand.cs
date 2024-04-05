using DomainDriverDesignAbstractions;
using MediatR;

namespace ProfileService.Application.Commands.AddDoomSlayerPoint;

public class AddDoomSlayerPointCommand : IRequest<Result>
{
    public string Email { get; set; }
}