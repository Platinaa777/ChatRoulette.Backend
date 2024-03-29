using MediatR;
using ProfileService.Domain.Shared;

namespace ProfileService.Application.Commands.AddFriendCommand;

public class AddFriendCommandHandler
    : IRequestHandler<AddFriendCommand, Result>
{
    public AddFriendCommandHandler()
    {
        
    }
    
    public Task<Result> Handle(AddFriendCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}