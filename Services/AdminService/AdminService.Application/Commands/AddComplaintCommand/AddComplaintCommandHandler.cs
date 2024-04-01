using DomainDriverDesignAbstractions;
using MediatR;

namespace AdminService.Application.Commands.AddComplaintCommand;

public class AddComplaintCommandHandler :
    IRequestHandler<AddComplaintCommand, Result>
{
    public AddComplaintCommandHandler()
    {
        
    }
    
    public Task<Result> Handle(AddComplaintCommand request, CancellationToken cancellationToken)
    {

        return Task.FromResult(Result.Success());
    }
}