using DomainDriverDesignAbstractions;
using MediatR;

namespace AdminService.Application.Commands.AddFeedbackCommand;

public class AddFeedbackCommandHandler
    : IRequestHandler<AddFeedbackCommand, Result>
{
    public AddFeedbackCommandHandler(
        )
    {
        
    }
    
    public Task<Result> Handle(AddFeedbackCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}