using DomainDriverDesignAbstractions;
using MediatR;

namespace AdminService.Application.Commands.AddFeedback;

public class AddFeedbackCommandHandler
    : IRequestHandler<AddFeedback.AddFeedbackCommand, Result>
{
    public AddFeedbackCommandHandler(
        )
    {
        
    }
    
    public Task<Result> Handle(AddFeedback.AddFeedbackCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}