using DomainDriverDesignAbstractions;
using MediatR;
using ProfileService.Application.Models;

namespace ProfileService.Application.Commands.ChangeAvatar;

public class ChangeAvatarCommand : IRequest<Result<AvatarInformation>>
{
    public Stream Avatar { get; set; }
    public string Email { get; set; }
    public string ContentType { get; set; }
}