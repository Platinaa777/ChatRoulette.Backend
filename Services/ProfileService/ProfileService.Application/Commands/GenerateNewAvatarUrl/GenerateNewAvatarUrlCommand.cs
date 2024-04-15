using DomainDriverDesignAbstractions;
using MediatR;
using ProfileService.Application.Models;

namespace ProfileService.Application.Commands.GenerateNewAvatarUrl;

public class GenerateNewAvatarUrlCommand : IRequest<Result<AvatarInformation>>
{
    public string Email { get; set; } = string.Empty;
}