using DomainDriverDesignAbstractions;
using MediatR;

namespace ProfileService.Application.Commands.DeleteFromFriendsCommand;

public class DeleteFromFriendsCommand
    : IRequest<Result>
{
    public string SenderEmail { get; set; }
    public string FriendEmail { get; set; }
}