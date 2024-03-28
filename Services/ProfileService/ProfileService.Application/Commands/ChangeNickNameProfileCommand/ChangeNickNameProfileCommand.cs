using MassTransit.Contracts.UserEvents;
using MediatR;
using ProfileService.Domain.Models.UserProfileAggregate;
using ProfileService.Domain.Shared;

namespace ProfileService.Application.Commands.ChangeNickNameProfileCommand;

public class ChangeNickNameProfileCommand : IRequest<Result>
{
    public string NickName { get; set; }
    public string Email { get; set; }
}